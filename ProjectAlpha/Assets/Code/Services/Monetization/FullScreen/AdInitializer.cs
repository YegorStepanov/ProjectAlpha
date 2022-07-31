using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Advertisements;

namespace Code.Services.Monetization;

public class AdInitializer : IUnityAdsInitializationListener
{
    private readonly string _gameId;

    public bool IsInitialized { get; private set; }

    public AdInitializer(AdsSettings settings)
    {
        _gameId = settings.GameId;
    }

    public UniTask InitializeAsync(CancellationToken token)
    {
        if (IsInitialized || token.IsCancellationRequested)
            return UniTask.CompletedTask;

        Advertisement.Initialize(_gameId, true, this);

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
}
