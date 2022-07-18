using System.Threading;
using Cysharp.Threading.Tasks;

namespace Code.Services.Monetization;

public interface IAdShow
{
    bool IsShowing { get; }
    UniTask ShowAsync(string adUnitId, CancellationToken token);
}
