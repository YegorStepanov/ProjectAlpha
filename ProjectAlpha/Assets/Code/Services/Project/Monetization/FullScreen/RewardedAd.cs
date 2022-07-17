namespace Code.Services.Monetization;

public class RewardedAd : Ad
{
    protected RewardedAd(IAdShow show, AdsSettings settings) :
        base(settings.RewardedId, show) { }
}
