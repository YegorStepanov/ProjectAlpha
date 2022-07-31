using System;
using System.Reflection;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Advertisements;

namespace Code.Services.Monetization;

public sealed class Ads : IAds, IDisposable
{
    private readonly AdInitializer _adInitializer;
    private readonly BannerAd _bannerAd;
    private readonly InterstitialAd _interstitialAd;
    private readonly RewardedAd _rewardedAd;

    public Ads(AdInitializer adInitializer, BannerAd bannerAd, InterstitialAd interstitialAd, RewardedAd rewardedAd)
    {
        _adInitializer = adInitializer;
        _bannerAd = bannerAd;
        _interstitialAd = interstitialAd;
        _rewardedAd = rewardedAd;
    }

    public void Dispose()
    {
        _bannerAd.Dispose();
    }

    private async UniTask InitializeAsync(CancellationToken token)
    {
        if (!_adInitializer.IsInitialized)
            await _adInitializer.InitializeAsync(token);
    }

    public async UniTask ShowBannerAsync(CancellationToken token)
    {
        await InitializeAsync(token);
        await _bannerAd.ShowAsync(token);
    }

    public async UniTask ShowInterstitialAsync(CancellationToken token)
    {
        await InitializeAsync(token);
        await ShowFullScreenAd(_interstitialAd, token);
    }

    public async UniTask<AdsShowResult> ShowRewardedAsync(CancellationToken token)
    {
        await InitializeAsync(token);
        return await ShowFullScreenAd(_rewardedAd, token);
    }

    public async UniTask HideBannerAsync(CancellationToken token)
    {
        await InitializeAsync(token);
        await _bannerAd.HideAsync(token);
    }

    private async UniTask<AdsShowResult> ShowFullScreenAd(FullScreenAd fullScreenAd, CancellationToken token)
    {
        bool isBannerActive = _bannerAd.IsShowing;
        if (isBannerActive) await HideBannerAsync(token);

        AdsShowResult result = await fullScreenAd.ShowAsync(token);

        if (isBannerActive) await ShowBannerAsync(token);

        return result;
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
