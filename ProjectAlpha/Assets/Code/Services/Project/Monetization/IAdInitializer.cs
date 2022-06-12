using System.Threading;
using Cysharp.Threading.Tasks;

namespace Code.Services.Monetization;

public interface IAdInitializer
{
    bool IsInitialized { get; }
    UniTask InitializeAsync(CancellationToken token);
}