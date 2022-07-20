using Code.Services;
using Code.Services.Game.UI;
using Cysharp.Threading.Tasks;

namespace Code.States;

public sealed class StartState : IState
{
    private readonly Camera _camera;
    private readonly IGameEvents _gameEvents;
    private readonly GameWorld _gameWorld;
    private readonly GameMediator _gameMediator;
    private readonly HeroSpawner _heroSpawner;
    private readonly PlatformSpawner _platformSpawner;

    public StartState(PlatformSpawner platformSpawner, HeroSpawner heroSpawner, Camera camera, IGameEvents gameEvents, GameWorld gameWorld, GameMediator gameMediator)
    {
        _platformSpawner = platformSpawner;
        _heroSpawner = heroSpawner;
        _camera = camera;
        _gameEvents = gameEvents;
        _gameWorld = gameWorld;
        _gameMediator = gameMediator;
    }

    public async UniTaskVoid EnterAsync(IStateMachine stateMachine)
    {
        await ChangeBackground();
        SwitchWorldHeight();
        ResetScore();

        IPlatform menuPlatform = await CreateMenuPlatform();
        IHero hero = CreateHero(menuPlatform);
        await WaitGameStartEvent();

        stateMachine.Enter<MoveHeroToPlatformState, MoveHeroToPlatformState.Arguments>(
            new(menuPlatform, menuPlatform, hero, null, new CherryNull())); //todo, reuse nullcherry
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
        await _gameEvents.GameStart.WaitAsync();
    }
}
