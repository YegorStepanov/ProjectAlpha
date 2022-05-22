using Code.Services;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.States;

public sealed class GameStartState : IArgState<GameStartState.Arguments>
{
    public readonly record struct Arguments(IPlatformController CurrentPlatform, IHeroController Hero);

    private readonly CameraController _cameraController;
    private readonly PlatformSpawner _platformSpawner;

    private readonly StickSpawner _stickSpawner;

    public GameStartState(
        CameraController cameraController,
        PlatformSpawner platformSpawner,
        StickSpawner stickSpawner)
    {
        _cameraController = cameraController;
        _platformSpawner = platformSpawner;
        _stickSpawner = stickSpawner;
    }

    public async UniTaskVoid EnterAsync(Arguments args, IStateMachine stateMachine)
    {
        await MoveHeroAsync(args.CurrentPlatform, args.Hero);

        await UniTask.Delay(100);
        UniTask moveCameraTask = MoveCameraAsync(args.CurrentPlatform);

        IPlatformController nextPlatform = await _platformSpawner.CreateAndMoveNextPlatformAsync(args.CurrentPlatform);

        await moveCameraTask;

        stateMachine.Enter<StickControlState, StickControlState.Arguments>(
            new StickControlState.Arguments(args.CurrentPlatform, nextPlatform, args.Hero));
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