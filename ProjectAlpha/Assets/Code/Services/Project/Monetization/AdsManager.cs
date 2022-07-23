using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Code.Services.Monetization;

public sealed class AdsManager : IDisposable
{
    private readonly Ads _ads;
    private readonly PlayerProgress _playerProgress;
    private readonly GameProgress _gameProgress;
    private readonly Settings _settings;
    private readonly CancellationToken _token;

    private readonly Action<bool> _adsEnabledChanged;
    private readonly Action<int> _restartNumberChanged;

    public AdsManager(Ads ads, PlayerProgress playerProgress, GameProgress gameProgress, Settings settings, ScopeToken token)
    {
        _ads = ads;
        _playerProgress = playerProgress;
        _gameProgress = gameProgress;
        _settings = settings;
        _token = token;

        _adsEnabledChanged = UniTaskHelper.Action<bool>(AdsEnabledChanged);
        _restartNumberChanged = UniTaskHelper.Action<int>(RestartNumberChanged);
        Init();
    }

    private void Init()
    {
        ShowBanner().Forget();
        _playerProgress.AdsEnabledChanged += _adsEnabledChanged;
        _gameProgress.RestartNumberChanged += _restartNumberChanged;
    }

    public void Dispose()
    {
        _playerProgress.AdsEnabledChanged -= _adsEnabledChanged;
        _gameProgress.RestartNumberChanged -= _restartNumberChanged;
    }

    public void WatchRewardedAd()
    {
        WatchRewardedAdImpl().Forget();
    }

    private async UniTaskVoid WatchRewardedAdImpl()
    {
        AdsShowResult result = await _ads.ShowRewardedAsync(_token);
        if (result == AdsShowResult.Completed)
            _playerProgress.AddCherries(_settings.CherriesForWatchingRewardedAds);
    }

    private async UniTaskVoid AdsEnabledChanged(bool adsEnabled)
    {
        await (adsEnabled ? ShowBanner() : HideBanner());
    }

    private UniTask ShowBanner()
    {
        return _ads.ShowBannerAsync(_token);
    }

    private UniTask HideBanner()
    {
        return _ads.HideBannerAsync(_token);
    }

    private async UniTaskVoid RestartNumberChanged(int restartNumber)
    {
        if (restartNumber % _settings.ShowAdsAfterRestartNumber == 0)
            await _ads.ShowInterstitialAsync(_token);
    }

    [System.Serializable]
    public class Settings
    {
        public int ShowAdsAfterRestartNumber = 3;
        public int CherriesForWatchingRewardedAds = 5;
    }
}
