using Code.Common;
using Code.Services;
using Code.Services.Entities;
using Code.Services.Spawners;
using Cysharp.Threading.Tasks;

namespace Code.States;

public sealed class NextRoundState : IState<NextRoundState.Arguments>
{
    public readonly record struct Arguments(IPlatform CurrentPlatform, IHero Hero);

    private readonly CameraMover _cameraMover;
    private readonly PlatformSpawner _platformSpawner;
    private readonly CherrySpawner _cherrySpawner;

    public NextRoundState(CameraMover cameraMover, PlatformSpawner platformSpawner, CherrySpawner cherrySpawner)
    {
        _cameraMover = cameraMover;
        _platformSpawner = platformSpawner;
        _cherrySpawner = cherrySpawner;
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
