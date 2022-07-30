using System;
using System.Threading;
using Code.Common;
using Code.Settings;
using Cysharp.Threading.Tasks;
using UnityEngine.Advertisements;

namespace Code.Services.Monetization.Banner;

public class BannerAd : IDisposable
{
    private const BannerPosition Position = BannerPosition.BOTTOM_CENTER;

    private readonly string _adUnitAd;
    private readonly BannerOptions _bannerOptions;

    public bool IsShowing { get; private set; }

    public BannerAd(AdsSettings settings)
    {
        _adUnitAd = settings.BannerId;

        _bannerOptions = new BannerOptions
        {
            showCallback = () => IsShowing = true,
            hideCallback = () => IsShowing = false,
        };
    }

    public void Dispose() =>
        DestroyBanner();

    public UniTask ShowAsync(CancellationToken token)
    {
        if (IsShowing || token.IsCancellationRequested)
            return UniTask.CompletedTask;

        Advertisement.Banner.SetPosition(Position);
        Advertisement.Banner.Show(_adUnitAd, _bannerOptions);

        return WaitForShowingAsync(token);
    }

    public UniTask HideAsync(CancellationToken token)
    {
        if (!IsShowing || token.IsCancellationRequested)
            return UniTask.CompletedTask;

        Advertisement.Banner.Hide();
        return WaitForHidingAsync(token);
    }

    private void DestroyBanner()
    {
        if (!IsShowing) return;

        //it's a bug or Hide(true) does nothing in editor
        if (PlatformInfo.IsEditor)
            Advertisement.Banner.Hide();

        Advertisement.Banner.Hide(true);
    }

    private async UniTask WaitForShowingAsync(CancellationToken token)
    {
        while (!IsShowing && !token.IsCancellationRequested)
            await UniTask.Yield(token);
    }

    private async UniTask WaitForHidingAsync(CancellationToken token)
    {
        while (IsShowing && !token.IsCancellationRequested)
            await UniTask.Yield(token);
    }
}
