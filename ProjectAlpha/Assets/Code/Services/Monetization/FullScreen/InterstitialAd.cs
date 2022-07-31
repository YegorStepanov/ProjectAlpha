namespace Code.Services.Monetization;

public class InterstitialAd : FullScreenAd
{
    protected InterstitialAd(AdsSettings settings) :
        base(settings.InterstitialId, new AdShow()) { }
}
