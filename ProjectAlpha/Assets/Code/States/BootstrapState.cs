using Code.Services;
using Code.Services.Game.UI;
using Cysharp.Threading.Tasks;

namespace Code.States;

//from menu
public sealed class BootstrapState : IState
{
    private readonly CameraController _cameraController;
    private readonly GameTriggers _gameTriggers;
    private readonly GameUIMediator _gameUI;
    private readonly CherrySpawner _cherrySpawner;
    private readonly StickSpawner _stickSpawner;
    private readonly HeroSpawner _heroSpawner;
    private readonly PlatformSpawner _platformSpawner;

    public BootstrapState(
        PlatformSpawner platformSpawner,
        HeroSpawner heroSpawner,
        CameraController cameraController,
        GameTriggers gameTriggers, 
        GameUIMediator gameUI,
        CherrySpawner cherrySpawner,
        StickSpawner stickSpawner)
    {
        _platformSpawner = platformSpawner;
        _heroSpawner = heroSpawner;
        _cameraController = cameraController;
        _gameTriggers = gameTriggers;
        _gameUI = gameUI;
        _cherrySpawner = cherrySpawner;
        _stickSpawner = stickSpawner;
    }

    private static int c = 0;
    
    public async UniTaskVoid EnterAsync(IStateMachine stateMachine)
    {
        _platformSpawner.DespawnAll();
        _cherrySpawner.DespawnAll();
        _stickSpawner.DespawnAll();
        
        _ = _gameUI.ShowHelp();
        
        UniTask loadBackgroundTask = _cameraController.ChangeBackgroundAsync();

        IPlatformController menuPlatform = await _platformSpawner.CreateMenuPlatformAsync();

        IHeroController hero = await _heroSpawner.CreateHeroAsync(menuPlatform.Position, Relative.Left);

        if(c++ == 0) //rework 
            await _gameTriggers.OnGameStarted.Await();

        await loadBackgroundTask;

        stateMachine.Enter<GameStartState, GameStartState.Arguments>(
            new GameStartState.Arguments(menuPlatform, menuPlatform, hero));

        //set idle animation
    }

    public void Exit() { }
}