using System.Threading;
using Cysharp.Threading.Tasks;

namespace Code.Services.Monetization;

public abstract class Ad
{
    private readonly string _adUnitAd;
    private readonly IAdShow _show;

    protected Ad(string adUnitAd, IAdShow show)
    {
        _adUnitAd = adUnitAd;
        _show = show;
    }

    public async UniTask ShowAsync(CancellationToken token) =>
        await _show.ShowAsync(_adUnitAd, token);
}
