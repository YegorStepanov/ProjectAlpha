using Code.Services;
using Code.Services.Game.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Camera = Code.Services.Camera;

namespace Code.States;

public sealed class GameStartState : IState<GameStartState.Arguments>
{
    public readonly record struct Arguments(IPlatform CurrentPlatform, IHero Hero);

    private readonly Camera _camera;
    private readonly PlatformSpawner _platformSpawner;
    private readonly CherrySpawner _cherrySpawner;
    private readonly PlatformPositionGenerator _platformPositionGenerator;
    private readonly CherryPositionGenerator _cherryPositionGenerator;
    private readonly GameMediator _gameMediator;
    private readonly GameWorld _gameWorld;

    public GameStartState(
        Camera camera,
        PlatformSpawner platformSpawner,
        CherrySpawner cherrySpawner,
        PlatformPositionGenerator platformPositionGenerator,
        CherryPositionGenerator cherryPositionGenerator,
        GameMediator gameMediator,
        GameWorld gameWorld)
    {
        _camera = camera;
        _platformSpawner = platformSpawner;
        _cherrySpawner = cherrySpawner;
        _platformPositionGenerator = platformPositionGenerator;
        _cherryPositionGenerator = cherryPositionGenerator;
        _gameMediator = gameMediator;
        _gameWorld = gameWorld;
    }

    public async UniTaskVoid EnterAsync(Arguments args, IStateMachine stateMachine)
    {
        _gameMediator.IncreaseScore(); //replace to Increase and Reset

        IPlatform currentPlatform = args.CurrentPlatform;
        Borders nextCameraBorders = NextCameraBorders(currentPlatform);

        IPlatform nextPlatform = await _platformSpawner.CreateAsync(nextCameraBorders.Right, Relative.Left);
        ICherry cherry = await _cherrySpawner.CreateAsync(nextCameraBorders.Right, Relative.RightTop);

        _gameWorld.SwitchToGameHeight();

        await UniTask.Delay(100);

        currentPlatform.RedPoint.FadeOutAsync();

        await (MoveCamera(nextCameraBorders),
            MovePlatformWithCherry(nextPlatform, cherry, currentPlatform, nextCameraBorders));

        stateMachine.Enter<StickControlState, StickControlState.Arguments>(
            new(currentPlatform, nextPlatform, args.Hero, cherry));
    }

    private Borders NextCameraBorders(IPlatform currentPlatform)
    {
        Vector2 offset = currentPlatform.Borders.LeftBot - _camera.Borders.LeftBot;
        return _camera.Borders.Shift(offset);
    }

    private UniTask MoveCamera(Borders nextCameraBorders)
    {
        return _camera.MoveAsync(nextCameraBorders.Center);
    }

    private async UniTask MovePlatformWithCherry(
        IPlatform nextPlatform, ICherry cherry, IPlatform currentPlatform, Borders nextCameraBorders)
    {
        float platformDestinationX = _platformPositionGenerator.NextPosition(
            currentPlatform.Borders.Right, nextCameraBorders.Right, nextPlatform.Borders.Width);

        float cherryDestinationX = _cherryPositionGenerator.NextPosition(
            currentPlatform.Borders.Right, platformDestinationX - nextPlatform.Borders.HalfWidth, cherry.Borders.Width);

        await (nextPlatform.MoveAsync(platformDestinationX), cherry.MoveAsync(cherryDestinationX));
    }
}

//hero is присидает каждый тик

//чел бежит к концу следующей платформы
//задний фон плывет
