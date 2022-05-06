using Code.Services;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.States;

public sealed class GameStartState : IArgState<GameStartState.Arguments>
{
    public readonly record struct Arguments(IPlatformController CurrentPlatform, IHeroController Hero);

    private readonly CameraController _cameraController;
    private readonly PlatformSpawner _platformSpawner;

    private readonly StickSpawner _stickSpawner;
    private readonly WidthGenerator _widthGenerator;

    public GameStartState(
        CameraController cameraController,
        PlatformSpawner platformSpawner,
        StickSpawner stickSpawner,
        WidthGenerator widthGenerator)
    {
        _cameraController = cameraController;
        _platformSpawner = platformSpawner;
        _stickSpawner = stickSpawner;
        _widthGenerator = widthGenerator;
    }

    public async UniTaskVoid EnterAsync(Arguments args, IStateMachine stateMachine)
    {
        await MoveHeroAsync(args.CurrentPlatform, args.Hero);

        await UniTask.Delay(100);
        UniTask moveCameraTask = MoveCameraAsync(args.CurrentPlatform);

        IPlatformController nextPlatform = CreateNextPlatform(args.CurrentPlatform);
        UniTask movePlatformTask = MoveNextPlatformToRandomPoint(args.CurrentPlatform, nextPlatform);

        await (moveCameraTask, movePlatformTask);

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

    private IPlatformController CreateNextPlatform(IPlatformController currentPlatform)
    {
        float leftCameraBorderToPlatformDistance = currentPlatform.Borders.Left - _cameraController.Borders.Left;
        Vector2 position = new(
            _cameraController.Borders.Right + leftCameraBorderToPlatformDistance,
            currentPlatform.Borders.Top);

        return _platformSpawner.CreatePlatform(position, _widthGenerator.NextWidth(), Relative.Left);
    }

    private static async UniTask MoveNextPlatformToRandomPoint(
        IPlatformController currentPlatform,
        IPlatformController nextPlatform)
    {
        float halfWidth = nextPlatform.Borders.Width / 2f;
        const float minDistance = 0.5f; //minOffset
        float posX = Random.Range(currentPlatform.Borders.Right + halfWidth + minDistance,
            nextPlatform.Borders.Left - halfWidth);

        int randDelay = Random.Range(0, 300); //ms
        await UniTask.Delay(randDelay);

        await nextPlatform.MoveAsync(posX);
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