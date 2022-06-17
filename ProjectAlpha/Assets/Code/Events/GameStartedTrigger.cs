using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;

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
        if (!_token.IsCancellationRequested)
        {
            await _stream.FirstAsync(_token).SuppressCancellationThrow();
        }
    }
}