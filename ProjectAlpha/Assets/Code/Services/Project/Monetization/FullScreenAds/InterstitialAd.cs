namespace Code.Services.Monetization;

public class InterstitialAd : FullScreenAd
{
    protected InterstitialAd(Ads.Settings settings) :
        base(settings.InterstitialId, new AdShow()) { }
}
