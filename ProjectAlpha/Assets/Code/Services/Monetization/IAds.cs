using System.Threading;
using Code.Services.Monetization.FullScreen;
using Cysharp.Threading.Tasks;

namespace Code.Services.Monetization;

public interface IAds
{
    UniTask ShowBannerAsync(CancellationToken token);
    UniTask ShowInterstitialAsync(CancellationToken token);
    UniTask<AdsShowResult> ShowRewardedAsync(CancellationToken token);
    UniTask HideBannerAsync(CancellationToken token);
}
