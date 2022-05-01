using Code.Services;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.States;

public sealed class BootstrapState : IState
{
    private readonly CameraController cameraController;
    private readonly GameTriggers gameTriggers;
    private readonly IHeroController hero;
    private readonly PlatformSpawner platformSpawner;
    private readonly StartSceneInformer startSceneInformer;
    private readonly GameStateMachine stateMachine;
    private readonly WidthGenerator widthGenerator;

    public BootstrapState(
        GameStateMachine stateMachine,
        PlatformSpawner platformSpawner,
        WidthGenerator widthGenerator,
        IHeroController hero,
        CameraController cameraController,
        GameTriggers gameTriggers,
        StartSceneInformer startSceneInformer)
    {
        this.stateMachine = stateMachine;
        this.platformSpawner = platformSpawner;
        this.widthGenerator = widthGenerator;
        this.hero = hero;
        this.cameraController = cameraController;
        this.gameTriggers = gameTriggers;
        this.startSceneInformer = startSceneInformer;
    }

    public async UniTaskVoid EnterAsync()
    {
        Debug.Log("BootstrapState.Enter" + ": " + Time.frameCount);
        widthGenerator.Reset();

        UniTask loadBackgroundTask = cameraController.ChangeBackgroundAsync();

        Vector2 platformPosition = cameraController.ViewportToWorldPosition(new Vector2(0.5f, 0.2f));
        IPlatformController menuPlatform = platformSpawner.CreatePlatform(platformPosition, 2f, Relative.Center);

        hero.TeleportTo(menuPlatform.Position, Relative.Left);

        if (!startSceneInformer.IsGameStartScene)
            await gameTriggers.StartGameTrigger.OnClickAsync();

        await loadBackgroundTask;

        stateMachine.Enter<GameStartState, GameStartState.Arguments>(
            new GameStartState.Arguments(menuPlatform));

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