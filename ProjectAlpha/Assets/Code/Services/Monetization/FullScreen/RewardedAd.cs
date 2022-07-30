using Code.Settings;

namespace Code.Services.Monetization.FullScreen;

public class RewardedAd : FullScreenAd
{
    protected RewardedAd(AdsSettings settings) :
        base(settings.RewardedId, new AdShow()) { }
}
