﻿using System.Threading;
using Cysharp.Threading.Tasks;

namespace Code.Services.Monetization;

public sealed class AdsManager
{
    private readonly IAds _ads;
    private readonly IProgress _progress;
    private readonly Settings _settings;
    private readonly CancellationToken _token;

    private bool AdsEnabled => _progress.Persistant.AdsEnabled;

    public AdsManager(IAds ads, IProgress progress, Settings settings, CancellationToken token)
    {
        _ads = ads;
        _progress = progress;
        _settings = settings;
        _token = token;
    }

    public void ShowBannerIfNeeded()
    {
        Impl().Forget();

        async UniTaskVoid Impl()
        {
            if (AdsEnabled)
                await _ads.ShowBannerAsync(_token);
            else
                await _ads.HideBannerAsync(_token);
        }
    }

    public void ShowInterstitialAdIfNeeded()
    {
        Impl().Forget();

        async UniTaskVoid Impl()
        {
            if (!AdsEnabled) return;

            int restartNumber = _progress.Session.RestartNumber;
            if (restartNumber % _settings.ShowAdsAfterRestartNumber == 0)
                await _ads.ShowInterstitialAsync(_token);
        }
    }

    public void ShowRewardedAd()
    {
        Impl().Forget();

        async UniTaskVoid Impl()
        {
            AdsShowResult result = await _ads.ShowRewardedAsync(_token);

            if (result == AdsShowResult.Completed)
                _progress.Persistant.AddCherries(_settings.CherriesForWatchingRewardedAds);
        }
    }

    [System.Serializable]
    public class Settings
    {
        public int ShowAdsAfterRestartNumber = 3;
        public int CherriesForWatchingRewardedAds = 5;
    }
}