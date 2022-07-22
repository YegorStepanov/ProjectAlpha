namespace Code.Services.Monetization;

public class RewardedAd : FullScreenAd
{
    protected RewardedAd(Ads.Settings settings) :
        base(settings.RewardedId, new AdShow()) { }
}
