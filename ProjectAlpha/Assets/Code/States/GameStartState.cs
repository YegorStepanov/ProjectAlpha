using Code.Services;
using Code.Services.Game.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.States;

public sealed class GameStartState : IState<GameStartState.Arguments>
{
    public readonly record struct Arguments(IPlatformController CurrentPlatform, IHeroController Hero);

    private readonly CameraController _cameraController;
    private readonly PlatformSpawner _platformSpawner;
    private readonly CherrySpawner _cherrySpawner;
    private readonly NextPositionGenerator _nextPositionGenerator;
    private readonly GameMediator _gameMediator;

    public GameStartState(
        CameraController cameraController,
        PlatformSpawner platformSpawner,
        CherrySpawner cherrySpawner,
        NextPositionGenerator nextPositionGenerator,
        GameMediator gameMediator)
    {
        _cameraController = cameraController;
        _platformSpawner = platformSpawner;
        _cherrySpawner = cherrySpawner;
        _nextPositionGenerator = nextPositionGenerator;
        _gameMediator = gameMediator;
    }

    public async UniTaskVoid EnterAsync(Arguments args, IStateMachine stateMachine)
    {
//        var arp = new AsyncReactiveProperty<int>(10);

        _gameMediator.IncreaseScore();

        await UniTask.Delay(100);

        Vector2 destination = args.CurrentPlatform.Borders.LeftBottom;
        var cameraDestination = destination.Shift(_cameraController.Borders, Relative.LeftBottom);
        var platformDestination = _cameraController.Borders.Right +
                                  (args.CurrentPlatform.Borders.Left - _cameraController.Borders.Left); //rework

        var moveCameraTask = _cameraController.MoveAsync(cameraDestination);

        _ = args.CurrentPlatform.FadeOutRedPointAsync();

        float nextPositionX = args.CurrentPlatform.Borders.Left + _cameraController.Borders.Width;

        IPlatformController nextPlatform = await _platformSpawner.CreatePlatformAsync(nextPositionX, Relative.Left);
        ICherryController cherry = await _cherrySpawner.CreateCherryAsync(nextPlatform);

        await MoveNextPlatformToRandomPoint(args.CurrentPlatform, nextPlatform, cherry, platformDestination);

        await moveCameraTask;

        stateMachine.Enter<StickControlState, StickControlState.Arguments>(
            new(args.CurrentPlatform, nextPlatform, args.Hero, cherry));
    }

    private async UniTask MoveNextPlatformToRandomPoint(
        IPlatformController currentPlatform,
        IPlatformController nextPlatform,
        ICherryController cherry,
        float cameraDestination)
    {
        float newPos = _nextPositionGenerator.NextPosition(currentPlatform, nextPlatform, cameraDestination);

        UniTask movePlatform = nextPlatform.MoveAsync(newPos);
        UniTask moveCherry = cherry.MoveRandomlyAsync(currentPlatform, newPos);

        await (movePlatform, moveCherry);
    }

    public void Exit() { }
}

//hero is присидает каждый тик

//чел бежит к концу следующей платформы
//задний фон плывет