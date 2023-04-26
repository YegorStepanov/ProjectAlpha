using System.Threading;
using Cysharp.Threading.Tasks;

namespace Code.Services.Monetization
{
    public abstract class FullScreenAd
    {
        private readonly string _adUnitAd;
        private readonly AdShow _show;

        public bool IsShowing => _show.IsShowing;

        protected FullScreenAd(string adUnitAd, AdShow show)
        {
            _adUnitAd = adUnitAd;
            _show = show;
        }

        public UniTask<AdsShowResult> ShowAsync(CancellationToken token) =>
            _show.ShowAsync(_adUnitAd, token);
    }
}
