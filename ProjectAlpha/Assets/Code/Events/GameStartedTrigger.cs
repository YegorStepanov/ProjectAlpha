using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UnityEngine;

namespace Code.Triggers;

public sealed class GameStartedTrigger
{
    private IUniTaskAsyncEnumerable<AsyncUnit> _events;
    private CancellationToken _token;

    public GameStartedTrigger(IUniTaskAsyncEnumerable<AsyncUnit> fallbackStream) =>
        SetTrigger(fallbackStream, CancellationToken.None);

    public void SetTrigger(IUniTaskAsyncEnumerable<AsyncUnit> stream, CancellationToken token)
    {
        _events = stream;
        _token = token;
    }

    public async UniTask Await()
    {
        if (_token.IsCancellationRequested) 
            Debug.LogError("GameStartedTrigger was cancelled");

        (bool isCanceled, _) = await _events.FirstAsync(_token).SuppressCancellationThrow();

        if (isCanceled) 
            Debug.LogError("GameStartedTrigger cancelled");
    }
}