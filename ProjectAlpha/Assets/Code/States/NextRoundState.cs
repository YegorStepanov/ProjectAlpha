using System;
using Code.Services;
using Code.Services.Game.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Camera = Code.Services.Camera;

namespace Code.States;

public sealed class NextRoundState : IState<NextRoundState.Arguments>
{
    public readonly record struct Arguments(IPlatform CurrentPlatform, IHero Hero);

    private readonly Camera _camera;
    private readonly PlatformSpawner _platformSpawner;
    private readonly CherrySpawner _cherrySpawner;
    private readonly PlatformPositionGenerator _platformPositionGenerator;
    private readonly CherryPositionGenerator _cherryPositionGenerator;
    private readonly GameWorld _gameWorld;
    private readonly GameStateMachine.Settings _settings;

    public NextRoundState(Camera camera, PlatformSpawner platformSpawner, CherrySpawner cherrySpawner, PlatformPositionGenerator platformPositionGenerator, CherryPositionGenerator cherryPositionGenerator, GameWorld gameWorld, GameStateMachine.Settings settings)
    {
        _camera = camera;
        _platformSpawner = platformSpawner;
        _cherrySpawner = cherrySpawner;
        _platformPositionGenerator = platformPositionGenerator;
        _cherryPositionGenerator = cherryPositionGenerator;
        _gameWorld = gameWorld;
        _settings = settings;
    }

    public async UniTaskVoid EnterAsync(Arguments args, IGameStateMachine stateMachine)
    {
        (IPlatform currentPlatform, IHero hero) = args;

        Borders nextCameraBorders = NextCameraBorders(currentPlatform);
        IPlatform nextPlatform = await CreatePlatform(nextCameraBorders);
        ICherry cherry = await CreateCherry(nextCameraBorders);
        SwitchWorldHeight();
        await Delay();
        await MoveCamera(nextCameraBorders, nextPlatform, cherry, currentPlatform);

        stateMachine.Enter<StickControlState, StickControlState.Arguments>(
            new(currentPlatform, nextPlatform, hero, cherry));
    }

    private Borders NextCameraBorders(IPlatform currentPlatform)
    {
        Vector2 offset = currentPlatform.Borders.LeftBot - _camera.Borders.LeftBot;
        return _camera.Borders.Shift(offset);
    }

    private UniTask<IPlatform> CreatePlatform(Borders nextCameraBorders)
    {
        return _platformSpawner.CreatePlatformAsync(nextCameraBorders.Right, Relative.Left);
    }

    private UniTask<ICherry> CreateCherry(Borders nextCameraBorders)
    {
        return _cherrySpawner.CreateAsync(nextCameraBorders.Right, Relative.RightTop);
    }

    private void SwitchWorldHeight()
    {
        _gameWorld.SwitchToGameHeight();
    }

    private UniTask Delay()
    {
        return UniTask.Delay(TimeSpan.FromSeconds(_settings.DelayBeforeNextState));
    }

    private async UniTask MoveCamera(Borders nextCameraBorders, IPlatform nextPlatform, ICherry cherry, IPlatform currentPlatform)
    {
        currentPlatform.RedPoint.FadeOutAsync().Forget();

        await (
            MoveCamera(nextCameraBorders),
            MovePlatformWithCherry(nextPlatform, cherry, currentPlatform, nextCameraBorders));
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
