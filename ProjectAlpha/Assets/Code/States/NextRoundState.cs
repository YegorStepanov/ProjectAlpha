using Code.Services;
using Code.Services.Game.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Camera = Code.Services.Camera;

namespace Code.States;

public sealed class NextRoundState : IState<NextRoundState.Arguments>
{
    public readonly record struct Arguments(IPlatform CurrentPlatform, IHero Hero);

    private readonly CameraMover _cameraMover;
    private readonly PlatformSpawner _platformSpawner;
    private readonly CherrySpawner _cherrySpawner;
    private readonly GameWorld _gameWorld;

    public NextRoundState(CameraMover cameraMover, PlatformSpawner platformSpawner, CherrySpawner cherrySpawner, GameWorld gameWorld)
    {
        _cameraMover = cameraMover;
        _platformSpawner = platformSpawner;
        _cherrySpawner = cherrySpawner;
        _gameWorld = gameWorld;
    }

    public async UniTaskVoid EnterAsync(Arguments args, IGameStateMachine stateMachine)
    {
        (IPlatform currentPlatform, IHero hero) = args;

        Borders nextCameraBorders = GetNextCameraBorders(currentPlatform);
        IPlatform nextPlatform = await CreatePlatform(nextCameraBorders);
        ICherry nextCherry = await CreateCherry(nextCameraBorders);

        currentPlatform.RedPoint.FadeOutAsync().Forget();
        await _cameraMover.MoveCamera(nextCameraBorders, currentPlatform, nextPlatform, nextCherry);

        stateMachine.Enter<StickControlState, StickControlState.Arguments>(
            new(currentPlatform, nextPlatform, hero, nextCherry));
    }

    private Borders GetNextCameraBorders(IPlatform currentPlatform)
    {
        return _cameraMover.GetNextCameraBorders(currentPlatform);
    }

    private UniTask<IPlatform> CreatePlatform(Borders nextCameraBorders) =>
        _platformSpawner.CreatePlatformAsync(nextCameraBorders.Right, Relative.Left);

    private UniTask<ICherry> CreateCherry(Borders nextCameraBorders) =>
        _cherrySpawner.CreateAsync(nextCameraBorders.Right, Relative.RightTop);
}

public class CameraMover
{
    private readonly Camera _camera;
    private readonly PlatformPositionGenerator _platformPositionGenerator;
    private readonly CherryPositionGenerator _cherryPositionGenerator;

    public CameraMover(Camera camera, PlatformPositionGenerator platformPositionGenerator, CherryPositionGenerator cherryPositionGenerator)
    {
        _camera = camera;
        _platformPositionGenerator = platformPositionGenerator;
        _cherryPositionGenerator = cherryPositionGenerator;
    }

    public Borders GetNextCameraBorders(IPlatform currentPlatform)
    {
        Vector2 offset = currentPlatform.Borders.LeftBot - _camera.Borders.LeftBot;
        return _camera.Borders.Shift(offset);
    }

    //destination instead next
    public async UniTask MoveCamera(Borders nextCameraBorders, IPlatform currentPlatform, IPlatform nextPlatform, ICherry nextCherry)
    {
        //split to MoveEntities/MovePlatformAndCherry?
        float platformDestinationX = _platformPositionGenerator.NextPosition(
            currentPlatform.Borders.Right, nextCameraBorders.Right, nextPlatform.Borders.Width);

        float cherryDestinationX = _cherryPositionGenerator.NextPosition(
            currentPlatform.Borders.Right, platformDestinationX - nextPlatform.Borders.HalfWidth, nextCherry.Borders.Width);

        await UniTask.WhenAll(
            _camera.MoveAsync(nextCameraBorders.Center),
            nextPlatform.MoveAsync(platformDestinationX),
            nextCherry.MoveAsync(cherryDestinationX));
    }
}
