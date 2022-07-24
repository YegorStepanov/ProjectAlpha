using System.Threading;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace Code.Services;

public struct ScopeCancellationToken
{
    private readonly CancellationToken _token;

    public ScopeCancellationToken(LifetimeScope scope) =>
        _token = scope.GetCancellationTokenOnDestroy();

    public static implicit operator CancellationToken(ScopeCancellationToken token) => token._token;
}
