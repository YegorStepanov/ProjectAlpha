using Code.Settings;

namespace Code.Services.Monetization.FullScreen;

public class InterstitialAd : FullScreenAd
{
    protected InterstitialAd(AdsSettings settings) :
        base(settings.InterstitialId, new AdShow()) { }
}
