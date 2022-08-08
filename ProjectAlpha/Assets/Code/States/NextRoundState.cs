using Code.Common;
using Code.Extensions;
using Code.Services;
using Code.Services.Entities;
using Code.Services.Infrastructure;
using Code.Services.Spawners;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.States;

public sealed class NextRoundState : IState<GameData>
{
    private readonly PlatformSpawner _platformSpawner;
    private readonly CherrySpawner _cherrySpawner;
    private readonly ICamera _camera;

    public NextRoundState(PlatformSpawner platformSpawner, CherrySpawner cherrySpawner, ICamera camera)
    {
        _platformSpawner = platformSpawner;
        _cherrySpawner = cherrySpawner;
        _camera = camera;
    }

    public async UniTaskVoid EnterAsync(GameData data, IGameStateMachine stateMachine)
    {
        Borders nextCameraBorders = GetNextCameraBorders(data.NextPlatform);

        Vector2 position = new(nextCameraBorders.Right, data.GameHeight.PositionY);
        (IPlatform nextPlatform, ICherry nextCherry) = await CreatePlatformAndCherry(position, data.GameHeight.Height);

        GameData nextData = data with
        {
            CurrentPlatform = data.NextPlatform,
            NextPlatform = nextPlatform,
            Cherry = nextCherry
        };

        stateMachine.Enter<CameraMovementState, (GameData, Vector2)>((nextData, nextCameraBorders.Center));
    }

    private Borders GetNextCameraBorders(IPlatform nextPlatform)
    {
        Vector2 offset = nextPlatform.Borders.LeftBot - _camera.Borders.LeftBot;
        return _camera.Borders.Shift(offset);
    }

    private UniTask<(IPlatform nextPlatform, ICherry nextCherry)> CreatePlatformAndCherry(Vector2 position, float height)
    {
        UniTask<IPlatform> nextPlatform = _platformSpawner.CreateGamePlatformAsync(position, height);
        UniTask<ICherry> nextCherry = _cherrySpawner.CreateAsync(position, Relative.RightTop);

        return UniTask.WhenAll(nextPlatform, nextCherry);
    }
}
