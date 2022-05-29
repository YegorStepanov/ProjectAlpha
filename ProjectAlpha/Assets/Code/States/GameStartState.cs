using Code.Services;
using Code.Services.Game.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.States;

public sealed class GameStartState : IArgState<GameStartState.Arguments>
{
    public readonly record struct Arguments(IPlatformController CurrentPlatform, IHeroController Hero);

    private readonly CameraController _cameraController;
    private readonly PlatformSpawner _platformSpawner;
    private readonly StickSpawner _stickSpawner;
    private readonly CherrySpawner _cherrySpawner;
    private readonly IPositionGenerator _positionGenerator;
    private readonly GameUIMediator _gameUI;

    public GameStartState(
        CameraController cameraController,
        PlatformSpawner platformSpawner,
        StickSpawner stickSpawner, CherrySpawner cherrySpawner, IPositionGenerator positionGenerator, GameUIMediator gameUI)
    {
        _cameraController = cameraController;
        _platformSpawner = platformSpawner;
        _stickSpawner = stickSpawner;
        _cherrySpawner = cherrySpawner;
        _positionGenerator = positionGenerator;
        _gameUI = gameUI;
    }

    public async UniTaskVoid EnterAsync(Arguments args, IStateMachine stateMachine)
    {
        await MoveHeroAsync(args.CurrentPlatform, args.Hero);
        _gameUI.IncreaseScore();
        _gameUI.IncreaseCherryCount();

        await UniTask.Delay(100);
        UniTask moveCameraTask = MoveCameraAsync(args.CurrentPlatform);

        _ = args.CurrentPlatform.FadeOutRedPointAsync();

        IPlatformController nextPlatform = await _platformSpawner.CreateNextPlatformAsync(args.CurrentPlatform);
        ICherryController cherry = await _cherrySpawner.CreateCherryAsync(args.CurrentPlatform, nextPlatform);

        await MoveNextPlatformToRandomPoint(args.CurrentPlatform, nextPlatform, cherry);

        await moveCameraTask;

        stateMachine.Enter<StickControlState, StickControlState.Arguments>(
            new StickControlState.Arguments(args.CurrentPlatform, nextPlatform, args.Hero));
    }

    private async UniTask MoveNextPlatformToRandomPoint(
        IPlatformController currentPlatform,
        IPlatformController nextPlatform, 
        ICherryController cherry)
    {
        float newPos = _positionGenerator.NextPosition(currentPlatform, nextPlatform);
        
        UniTask movePlatform = nextPlatform.MoveAsync(newPos);
        float end = newPos - nextPlatform.Borders.Width / 2f;
        UniTask moveCherry = cherry.MoveRandomlyAsync(currentPlatform, end);

        await (movePlatform, moveCherry);
    }

    public void Exit() { }

    private async UniTask MoveCameraAsync(IPlatformController currentPlatform)
    {
        Vector2 destination = new(currentPlatform.Borders.Left, currentPlatform.Borders.Bottom);
        await _cameraController.MoveAsync(destination, Relative.LeftBottom);
    }

    private async UniTask MoveHeroAsync(IPlatformController currentPlatform, IHeroController hero)
    {
        float destX = currentPlatform.Borders.Right;
        destX -= _stickSpawner.StickWidth / 2f;
        destX -= hero.HandOffset;
        await UniTask.Delay(200);
        await hero.MoveAsync(destX);
    }
}

public sealed class StickBuildingState
{
    //hero is присидает каждый тик
    //палка увеличивается линейно

    //next state
    //бьет ногой
    //палка поворачивается
    //

    //чел бежит к концу следующей платформы
    //задний фон плывет

    //пододвинуть камеру к началу новой платформы
}