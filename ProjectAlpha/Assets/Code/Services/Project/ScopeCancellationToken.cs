using System.Threading;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace Code.Services;

public class ScopeCancellationToken
{
    public CancellationToken Token { get; }

    // ReSharper disable once SuggestBaseTypeForParameterInConstructor
    public ScopeCancellationToken(LifetimeScope scope) =>
        Token = scope.GetCancellationTokenOnDestroy();
}