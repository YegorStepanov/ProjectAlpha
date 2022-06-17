using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Code.Services.Monetization;

public sealed class Ads : IDisposable
{
    private readonly IAdInitializer _adInitializer;
    private readonly BannerAd _bannerAd;
    private readonly InterstitialAd _interstitialAd;
    private readonly RewardedAd _rewardedAd;
    private readonly PlayerProgress _playerProgress;

    public Ads(
        IAdInitializer adInitializer,
        BannerAd bannerAd,
        InterstitialAd interstitialAd,
        RewardedAd rewardedAd,
        PlayerProgress playerProgress,
        ScopeCancellationToken token)
    {
        _adInitializer = adInitializer;
        _bannerAd = bannerAd;
        _interstitialAd = interstitialAd;
        _rewardedAd = rewardedAd;
        _playerProgress = playerProgress;
        Initialize(token.Token);
    }

    private void Initialize(CancellationToken token) =>
        _adInitializer.InitializeAsync(token);

    public async UniTask ShowBannerAsync(CancellationToken token)
    {
        if (_playerProgress.IsNoAds) return;
        await _adInitializer.InitializeAsync(token);
        await _bannerAd.ShowAsync(token);
    }

    public async UniTask ShowInterstitialAsync(CancellationToken token)
    {
        if (_playerProgress.IsNoAds) return;
        await _adInitializer.InitializeAsync(token);
        await ShowFullScreenAdAsync(_interstitialAd, token);
    }

    public async UniTask ShowRewardedAsync(CancellationToken token)
    {
        await _adInitializer.InitializeAsync(token);
        await ShowFullScreenAdAsync(_rewardedAd, token);
    }

    public async UniTask HideBannerAsync(CancellationToken token)
    {
        await _adInitializer.InitializeAsync(token);
        await _bannerAd.ShowAsync(token);
    }

    private async UniTask ShowFullScreenAdAsync(Ad ad, CancellationToken token)
    {
        if (token.IsCancellationRequested) return;
        await _bannerAd.HideAsync(token);
        await ad.ShowAsync(token);
        await _bannerAd.ShowAsync(token);
    }

    public void Dispose() =>
        _bannerAd.Destroy();
}