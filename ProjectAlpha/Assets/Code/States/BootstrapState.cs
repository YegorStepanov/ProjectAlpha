using Code.Services;
using Code.Services.Game.UI;
using Cysharp.Threading.Tasks;

namespace Code.States;

//from menu
public sealed class BootstrapState : IState
{
    private readonly CameraController _camera;
    private readonly GameTriggers _gameTriggers;
    private readonly GameData _gameData;
    private readonly HeroSpawner _heroSpawner;
    private readonly PlatformSpawner _platformSpawner;

    public BootstrapState(
        PlatformSpawner platformSpawner,
        HeroSpawner heroSpawner,
        CameraController camera,
        GameTriggers gameTriggers,
        GameData gameData)
    {
        _platformSpawner = platformSpawner;
        _heroSpawner = heroSpawner;
        _camera = camera;
        _gameTriggers = gameTriggers;
        _gameData = gameData;
    }

    public async UniTaskVoid EnterAsync(IStateMachine stateMachine)
    {
        _gameData.ChangeToMenuHeight();

        await _camera.ChangeBackgroundAsync();

        IPlatformController menuPlatform = await _platformSpawner.CreateMenuPlatformAsync();

        IHeroController hero = await _heroSpawner.CreateHeroAsync(menuPlatform.Borders.CenterTop, Relative.Left);

        await _gameTriggers.OnGameStarted.Await();

        // _gameData.ChangeToGameHeight();

        stateMachine.Enter<HeroMovementState, HeroMovementState.Arguments>(
            new(false, menuPlatform, menuPlatform, hero, null, null));
    }

    public void Exit() { }
}