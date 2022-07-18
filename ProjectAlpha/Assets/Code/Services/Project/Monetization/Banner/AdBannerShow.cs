using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Advertisements;

namespace Code.Services.Monetization;

public class AdBannerShow : IAdBannerShow
{
    private const BannerPosition Position = BannerPosition.BOTTOM_CENTER;

    private readonly BannerOptions _bannerOptions;

    public bool IsShowing { get; private set; }

    public AdBannerShow()
    {
        _bannerOptions = new BannerOptions
        {
            showCallback = Showed,
            hideCallback = Hidden,
            clickCallback = Clicked,
        };
    }

    public UniTask ShowAsync(string adUnitId, CancellationToken token)
    {
        if (token.IsCancellationRequested)
            return UniTask.CompletedTask;

        if (!IsShowing)
        {
            Advertisement.Banner.SetPosition(Position);
            Advertisement.Banner.Show(adUnitId, _bannerOptions);
        }

        return WaitForShowAsync(token);
    }

    private async UniTask WaitForShowAsync(CancellationToken token)
    {
        while (!IsShowing && !token.IsCancellationRequested)
            await UniTask.Yield(token);
    }

    private void Showed() => IsShowing = true;
    private void Hidden() => IsShowing = false;
    private void Clicked() => Debug.Log("Banner clicked");
}
