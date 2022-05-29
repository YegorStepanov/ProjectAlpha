using Code.Services;
using Code.Services.Game.UI;
using Cysharp.Threading.Tasks;

namespace Code.States;

public sealed class BootstrapState : IState
{
    private readonly CameraController _cameraController;
    private readonly GameTriggers _gameTriggers;
    private readonly GameUIMediator _gameUI;
    private readonly HeroSpawner _heroSpawner;
    private readonly PlatformSpawner _platformSpawner;

    public BootstrapState(
        PlatformSpawner platformSpawner,
        HeroSpawner heroSpawner,
        CameraController cameraController,
        GameTriggers gameTriggers, 
        GameUIMediator gameUI)
    {
        _platformSpawner = platformSpawner;
        _heroSpawner = heroSpawner;
        _cameraController = cameraController;
        _gameTriggers = gameTriggers;
        _gameUI = gameUI;
    }

    public async UniTaskVoid EnterAsync(IStateMachine stateMachine)
    {
        _ = _gameUI.ShowHelp();
        
        UniTask loadBackgroundTask = _cameraController.ChangeBackgroundAsync();

        IPlatformController menuPlatform = await _platformSpawner.CreateMenuPlatformAsync();

        IHeroController hero = await _heroSpawner.CreateHeroAsync(menuPlatform.Position, Relative.Left);

        await _gameTriggers.OnGameStarted.Await();

        await loadBackgroundTask;

        stateMachine.Enter<GameStartState, GameStartState.Arguments>(
            new GameStartState.Arguments(menuPlatform, hero));

        //set idle animation
    }

    public void Exit() { }
}