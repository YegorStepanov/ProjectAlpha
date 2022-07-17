namespace Code.Services.Monetization;

public class InterstitialAd : Ad
{
    protected InterstitialAd(IAdShow show, AdsSettings settings) :
        base(settings.InterstitialId, show) { }
}
