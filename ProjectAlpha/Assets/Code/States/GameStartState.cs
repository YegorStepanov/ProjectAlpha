using Code.Services;
using Code.Services.Game.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Camera = Code.Services.Camera;

namespace Code.States;

public sealed class GameStartState : IState<GameStartState.Arguments>
{
    public readonly record struct Arguments(IPlatform CurrentPlatform, IHero Hero, IStick Stick);

    private readonly Camera _camera;
    private readonly PlatformSpawner _platformSpawner;
    private readonly CherrySpawner _cherrySpawner;
    private readonly NextPositionGenerator _nextPositionGenerator;
    private readonly GameMediator _gameMediator;

    public GameStartState(
        Camera camera,
        PlatformSpawner platformSpawner,
        CherrySpawner cherrySpawner,
        NextPositionGenerator nextPositionGenerator,
        GameMediator gameMediator)
    {
        _camera = camera;
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
        var cameraDestination = destination.Shift(_camera.Borders, Relative.LeftBottom);
        var platformDestination = _camera.Borders.Right +
                                  (args.CurrentPlatform.Borders.Left - _camera.Borders.Left); //rework

        var moveCameraTask = _camera.MoveAsync(cameraDestination);

        _ = args.CurrentPlatform.FadeOutRedPointAsync();

        float nextPositionX = args.CurrentPlatform.Borders.Left + _camera.Borders.Width;

        IPlatform nextPlatform = await _platformSpawner.CreatePlatformAsync(nextPositionX, Relative.Left);
        ICherry cherry = await _cherrySpawner.CreateCherryAsync(nextPlatform);

        await MoveNextPlatformToRandomPoint(args.CurrentPlatform, nextPlatform, cherry, platformDestination);

        await moveCameraTask;

        stateMachine.Enter<StickControlState, StickControlState.Arguments>(
            new(args.CurrentPlatform, nextPlatform, args.Hero, cherry, args.Stick));
    }

    private async UniTask MoveNextPlatformToRandomPoint(
        IPlatform currentPlatform,
        IPlatform nextPlatform,
        ICherry cherry,
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