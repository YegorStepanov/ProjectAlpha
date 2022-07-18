using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Advertisements;

namespace Code.Services.Monetization;

public class AdShow : IAdShow, IUnityAdsShowListener
{
    public bool IsShowing { get; private set; }

    public UniTask ShowAsync(string adUnitId, CancellationToken token)
    {
        if (token.IsCancellationRequested)
            return UniTask.CompletedTask;

        if (!IsShowing)
            Advertisement.Show(adUnitId, this);

        return WaitForCompleteAsync(token);
    }

    private async UniTask WaitForCompleteAsync(CancellationToken token)
    {
        IsShowing = true;
        while (IsShowing && !token.IsCancellationRequested)
            await UniTask.Yield(token);
    }

    void IUnityAdsShowListener.OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.LogError($"Show failed: placementId={placementId} error={error}, message={message}");
    }

    void IUnityAdsShowListener.OnUnityAdsShowStart(string placementId) => Debug.Log("Show Start");
    void IUnityAdsShowListener.OnUnityAdsShowClick(string placementId) => Debug.Log("Show Click");

    void IUnityAdsShowListener.OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState state)
    {
        IsShowing = false;
    }
}
