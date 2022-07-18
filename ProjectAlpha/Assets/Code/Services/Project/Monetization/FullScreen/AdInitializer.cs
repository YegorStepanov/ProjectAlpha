using System;
using System.Reflection;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Advertisements;

namespace Code.Services.Monetization;

public class AdInitializer : IAdInitializer, IUnityAdsInitializationListener
{
    private readonly string _gameId;

    public bool IsInitialized { get; private set; }

    public AdInitializer(AdsSettings settings)
    {
        _gameId = settings.GameId;
    }

    public UniTask InitializeAsync(CancellationToken token)
    {
        if (token.IsCancellationRequested)
            return UniTask.CompletedTask;

        if (!IsInitialized)
            Advertisement.Initialize(_gameId, false, this);

        return WaitForInitializationAsync(token);
    }

    private async UniTask WaitForInitializationAsync(CancellationToken token)
    {
        while (!IsInitialized && !token.IsCancellationRequested)
            await UniTask.Yield(token);
    }

    void IUnityAdsInitializationListener.OnInitializationComplete() => IsInitialized = true;

    void IUnityAdsInitializationListener.OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.LogError($"Initialization failed: error={error}, message={message}");
    }

#if UNITY_EDITOR
    // ads package not working when domain reloading is disabled
    // https://forum.unity.com/threads/unity-ads-bug-missing-reference-error-and-no-ads-shows-up-after-reopening-the-game.1101634/
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void FixUnityAds()
    {
        Type type = typeof(Advertisement);
        FieldInfo sPlatform = type.GetField("s_Platform", BindingFlags.Static | BindingFlags.NonPublic);
        sPlatform!.SetValue(null, null);

        type.TypeInitializer.Invoke(null, null);
    }
#endif
}
