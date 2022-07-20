using System.Threading;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace Code.Services;

public class ScopeToken //change to struct?
{
    private readonly CancellationToken _token;

    public ScopeToken(LifetimeScope scope) =>
        _token = scope.GetCancellationTokenOnDestroy();

    public static implicit operator CancellationToken(ScopeToken scopeToken) => scopeToken._token;
}
