using System.ComponentModel;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Advertisements;

namespace Code.Services.Monetization
{
    public sealed class AdShow : IUnityAdsShowListener
    {
        public bool IsShowing { get; private set; }

        private AdsShowResult _showResult;

        public async UniTask<AdsShowResult> ShowAsync(string adUnitId, CancellationToken token)
        {
            if (IsShowing || token.IsCancellationRequested)
                return AdsShowResult.NotShown;

            Advertisement.Show(adUnitId, this);
            await WaitForCompleteAsync(token);

            AdsShowResult result = _showResult;
            _showResult = default;
            return result;
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

        void IUnityAdsShowListener.OnUnityAdsShowStart(string placementId) { }
        void IUnityAdsShowListener.OnUnityAdsShowClick(string placementId) { }
        void IUnityAdsShowListener.OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState state)
        {
            IsShowing = false;

            _showResult = state switch
            {
                UnityAdsShowCompletionState.SKIPPED => AdsShowResult.Skipped,
                UnityAdsShowCompletionState.COMPLETED => AdsShowResult.Completed,
                UnityAdsShowCompletionState.UNKNOWN => AdsShowResult.Unknown,
                _ => throw new InvalidEnumArgumentException(nameof(state), (int)state, typeof(UnityAdsShowCompletionState))
                };
        }
    }
}