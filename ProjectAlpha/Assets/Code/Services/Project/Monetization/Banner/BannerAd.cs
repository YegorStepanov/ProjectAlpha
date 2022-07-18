using System.Threading;
using Code.Infrastructure;
using Cysharp.Threading.Tasks;
using UnityEngine.Advertisements;

namespace Code.Services.Monetization;

//after move to menu -> show ads 50%, can be disabled
//bot of cress ads, can be disabled
//get 5 cherry -> ads

public class BannerAd
{
    private readonly string _adUnitAd;
    private readonly IAdBannerShow _show;

    public BannerAd(IAdBannerShow show, AdsSettings settings)
    {
        _adUnitAd = settings.BannerId;
        _show = show;
    }

    public void Dispose() =>
        Destroy();

    public UniTask ShowAsync(CancellationToken token) =>
        _show.ShowAsync(_adUnitAd, token);

    public UniTask HideAsync(CancellationToken token)
    {
        if (token.IsCancellationRequested)
            return UniTask.CompletedTask;

        Advertisement.Banner.Hide();
        return WaitForHidingAsync();
    }

    public void Destroy()
    {
        //it's a bug or Hide(true) does nothing in editor
        if (PlatformInfo.IsEditor)
            Advertisement.Banner.Hide();

        Advertisement.Banner.Hide(true);
    }

    private async UniTask WaitForHidingAsync()
    {
        while (_show.IsShowing)
            await UniTask.Yield();
    }
}
