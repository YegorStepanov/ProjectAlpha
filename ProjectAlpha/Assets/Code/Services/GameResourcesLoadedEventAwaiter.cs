using System.Threading;
using Code.Common;
using Cysharp.Threading.Tasks;
using MessagePipe;

namespace Code.Services ;

    public sealed class GameResourcesLoadedEventAwaiter
    {
        private readonly ISubscriber<Event.GameResourcesLoaded> _event;
        private readonly CancellationToken _token;

        public GameResourcesLoadedEventAwaiter(ISubscriber<Event.GameResourcesLoaded> @event, CancellationToken token)
        {
            _event = @event;
            _token = token;
        }

        public async UniTask Wait()
        {
            await _event.FirstAsync(_token);
        }
    }