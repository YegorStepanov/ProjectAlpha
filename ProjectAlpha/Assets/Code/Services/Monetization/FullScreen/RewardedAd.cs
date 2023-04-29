namespace Code.Services.Monetization
{
    public class RewardedAd : FullScreenAd
    {
        protected RewardedAd(AdsSettings settings) :
            base(settings.RewardedId, new AdShow()) { }
    }
}