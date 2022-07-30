using Code.Common;
using Code.Data.PositionGenerator;
using Code.Extensions;
using Code.Services;
using Code.Services.Entities.Cherry;
using Code.Services.Entities.Hero;
using Code.Services.Entities.Platform;
using Code.Services.Infrastructure;
using Code.Services.Spawners;
using Cysharp.Threading.Tasks;
using UnityEngine;

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

        currentPlatform.PlatformRedPoint.FadeOutAsync().Forget();
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
    private readonly ICamera _camera1;
    private readonly PlatformPositionGenerator _platformPositionGenerator;
    private readonly CherryPositionGenerator _cherryPositionGenerator;

    public CameraMover(ICamera camera1, PlatformPositionGenerator platformPositionGenerator, CherryPositionGenerator cherryPositionGenerator)
    {
        _camera1 = camera1;
        _platformPositionGenerator = platformPositionGenerator;
        _cherryPositionGenerator = cherryPositionGenerator;
    }

    public Borders GetNextCameraBorders(IPlatform currentPlatform)
    {
        Vector2 offset = currentPlatform.Borders.LeftBot - _camera1.Borders.LeftBot;
        return _camera1.Borders.Shift(offset);
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
            _camera1.MoveAsync(nextCameraBorders.Center),
            nextPlatform.MoveAsync(platformDestinationX),
            nextCherry.MoveAsync(cherryDestinationX));
    }
}
