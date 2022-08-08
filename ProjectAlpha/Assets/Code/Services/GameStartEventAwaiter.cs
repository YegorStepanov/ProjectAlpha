using System.Threading;
using Code.Common;
using Code.Services.Infrastructure;
using Cysharp.Threading.Tasks;
using MessagePipe;

namespace Code.Services;

public sealed class GameStartEventAwaiter
{
    private readonly ISubscriber<Event.GameStart> _gameStartEvent;
    private readonly ISceneLoader _sceneLoader;
    private readonly CancellationToken _token;

    public GameStartEventAwaiter(ISubscriber<Event.GameStart> gameStartEvent, ISceneLoader sceneLoader, CancellationToken token)
    {
        _gameStartEvent = gameStartEvent;
        _sceneLoader = sceneLoader;
        _token = token;
    }

    public async UniTask Wait()
    {
        //Skip awaiting when menu/bootstrap is not loaded,
        //a workaround for Editor, when the game is started from GameScene:
        if (_sceneLoader.IsLoaded<MenuScene>() || _sceneLoader.IsLoaded<BootstrapScene>())
            await _gameStartEvent.FirstAsync(_token);
    }
}
