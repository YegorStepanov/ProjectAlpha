using Code.Services;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.States;

public sealed class BootstrapState : IState
{
    private readonly CameraController _cameraController;
    private readonly GameTriggers _gameTriggers;
    private readonly AsyncInject<IHeroController> _hero;
    private readonly PlatformSpawner _platformSpawner;
    private readonly StartSceneInformer _startSceneInformer;
    private readonly GameStateMachine _stateMachine;
    private readonly WidthGenerator _widthGenerator;

    public BootstrapState(
        GameStateMachine stateMachine,
        PlatformSpawner platformSpawner,
        WidthGenerator widthGenerator,
        AsyncInject<IHeroController> hero,
        CameraController cameraController,
        GameTriggers gameTriggers,
        StartSceneInformer startSceneInformer)
    {
        _stateMachine = stateMachine;
        _platformSpawner = platformSpawner;
        _widthGenerator = widthGenerator;
        _hero = hero;
        _cameraController = cameraController;
        _gameTriggers = gameTriggers;
        _startSceneInformer = startSceneInformer;
    }

    public async UniTaskVoid EnterAsync()
    {
        IHeroController hero = await _hero; //hm, interesting mb add another service that start game after ALL are loaded
        Debug.Log("BootstrapState.Enter" + ": " + Time.frameCount);
        //_widthGenerator.Reset();

        UniTask loadBackgroundTask = _cameraController.ChangeBackgroundAsync();

        Vector2 platformPosition = _cameraController.ViewportToWorldPosition(new Vector2(0.5f, 0.2f));
        IPlatformController menuPlatform = _platformSpawner.CreatePlatform(platformPosition, 2f, Relative.Center);

        hero.TeleportTo(menuPlatform.Position, Relative.Left);

        if (!_startSceneInformer.IsGameStartScene)
            await _gameTriggers.StartGameTrigger.OnClickAsync();

        await loadBackgroundTask;

        _stateMachine.Enter<GameStartState, GameStartState.Arguments>(
            new GameStartState.Arguments(menuPlatform, hero));

        //set background randomly
        //set idle animation

        // hero = Object.Instantiate(heroPrefab, t1.position, Quaternion.identity);
        // Vector2 worldSize = SpriteHelper.WorldSpriteSize(hero.GetChild(0).GetComponent<SpriteRenderer>().sprite, hero.lossyScale);
        //
        // Vector3 pos = hero.position;
        // pos.x += worldSize.x / 2f;
        // hero.position = pos;
    }

    public void Exit() { }
}