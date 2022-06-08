using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UnityEngine;

namespace Code.Triggers;

public sealed class GameStartedTrigger
{
    private IUniTaskAsyncEnumerable<AsyncUnit> _stream;
    private CancellationToken _token;

    public GameStartedTrigger(IUniTaskAsyncEnumerable<AsyncUnit> fallbackStream) =>
        SetTrigger(fallbackStream, CancellationToken.None);

    public void SetTrigger(IUniTaskAsyncEnumerable<AsyncUnit> stream, CancellationToken token)
    {
        _stream = stream;
        _token = token;
    }

    public async UniTask WaitAsync()
    {
        if (_token.IsCancellationRequested) 
            Debug.LogError("GameStartedTrigger was cancelled");

        (bool isCanceled, _) = await _stream.FirstAsync(_token).SuppressCancellationThrow();

        if (isCanceled) 
            Debug.LogError("GameStartedTrigger cancelled");
    }
}