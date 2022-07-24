using System.Threading;
using Code.Services;
using Code.Services.Game.UI;
using Cysharp.Threading.Tasks;
using MessagePipe;

namespace Code.States;

public sealed class StartState : IState
{
    private readonly Camera _camera;
    private readonly ISubscriber<Event.GameStart> _gameStartEvent;
    private readonly GameWorld _gameWorld;
    private readonly GameMediator _gameMediator;
    private readonly CancellationToken _token;
    private readonly HeroSpawner _heroSpawner;
    private readonly ISceneLoader _sceneLoader;
    private readonly PlatformSpawner _platformSpawner;

    public StartState(ISceneLoader sceneLoader, PlatformSpawner platformSpawner, HeroSpawner heroSpawner, Camera camera, ISubscriber<Event.GameStart> gameStartEvent, GameWorld gameWorld, GameMediator gameMediator, CancellationToken token)
    {
        _sceneLoader = sceneLoader;
        _platformSpawner = platformSpawner;
        _heroSpawner = heroSpawner;
        _camera = camera;
        _gameStartEvent = gameStartEvent;
        _gameWorld = gameWorld;
        _gameMediator = gameMediator;
        _token = token;
    }

    public async UniTaskVoid EnterAsync(IGameStateMachine stateMachine)
    {
        await ChangeBackground();

        SwitchWorldHeight();
        ResetScore();

        IPlatform menuPlatform = await CreateMenuPlatform();
        IHero hero = CreateHero(menuPlatform);
        await WaitGameStartEvent();

        stateMachine.Enter<MoveHeroToPlatformState, MoveHeroToPlatformState.Arguments>(
            new(menuPlatform, menuPlatform, hero, StickNull.Default, CherryNull.Default));
    }

    private UniTask ChangeBackground()
    {
        return _camera.ChangeBackgroundAsync();
    }

    private void SwitchWorldHeight()
    {
        _gameWorld.SwitchToMenuHeight();
    }

    private void ResetScore()
    {
        _gameMediator.ResetScore();
    }

    private UniTask<IPlatform> CreateMenuPlatform()
    {
        return _platformSpawner.CreateMenuPlatformAsync();
    }

    private IHero CreateHero(IPlatform menuPlatform)
    {
        return _heroSpawner.Create(menuPlatform.Borders.CenterTop, Relative.Left);
    }

    private async UniTask WaitGameStartEvent()
    {
        if(_sceneLoader.IsLoaded<MenuScene>())
           await _gameStartEvent.FirstAsync(_token);
    }
}
