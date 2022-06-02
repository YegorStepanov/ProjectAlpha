using Code.Services;
using Code.Services.Game.UI;
using Cysharp.Threading.Tasks;

namespace Code.States;

//from menu
public sealed class BootstrapState : IState
{
    private readonly Camera _camera;
    private readonly GameTriggers _gameTriggers;
    private readonly GameData _gameData;
    private readonly StickSpawner _stickSpawner;
    private readonly HeroSpawner _heroSpawner;
    private readonly PlatformSpawner _platformSpawner;

    public BootstrapState(
        PlatformSpawner platformSpawner,
        HeroSpawner heroSpawner,
        Camera camera,
        GameTriggers gameTriggers,
        GameData gameData,
        StickSpawner stickSpawner)
    {
        _platformSpawner = platformSpawner;
        _heroSpawner = heroSpawner;
        _camera = camera;
        _gameTriggers = gameTriggers;
        _gameData = gameData;
        _stickSpawner = stickSpawner;
    }

    public async UniTaskVoid EnterAsync(IStateMachine stateMachine)
    {
        _gameData.ChangeToMenuHeight();

        await _camera.ChangeBackgroundAsync();

        IPlatform menuPlatform = await _platformSpawner.CreateMenuPlatformAsync();

        IHero hero = await _heroSpawner.CreateHeroAsync(menuPlatform.Borders.CenterTop, Relative.Left);

        await _gameTriggers.OnGameStarted.Await();

        // _gameData.ChangeToGameHeight();

        
        IStick stick = await _stickSpawner.CreateStickAsync(menuPlatform.Borders.RightTop); //old args.currentPlatform

        stateMachine.Enter<HeroMovementState, HeroMovementState.Arguments>(
            new(false, menuPlatform, menuPlatform, hero, null, stick));
    }

    public void Exit() { }
}