using Code.Services;
using Code.Services.Game.UI;
using Cysharp.Threading.Tasks;

namespace Code.States;

public sealed class RestartState : IState
{
    private readonly PlatformSpawner _platformSpawner;
    private readonly CherrySpawner _cherrySpawner;
    private readonly StickSpawner _stickSpawner;
    private readonly Camera _camera;
    private readonly HeroSpawner _heroSpawner;
    private readonly GameMediator _gameMediator;

    public RestartState(PlatformSpawner platformSpawner, CherrySpawner cherrySpawner, StickSpawner stickSpawner, Camera camera, HeroSpawner heroSpawner, GameMediator gameMediator)
    {
        _platformSpawner = platformSpawner;
        _cherrySpawner = cherrySpawner;
        _stickSpawner = stickSpawner;
        _camera = camera;
        _heroSpawner = heroSpawner;
        _gameMediator = gameMediator;
    }

    public async UniTaskVoid EnterAsync(IStateMachine stateMachine)
    {
        await ChangeBackground();
        ResetCamera();
        ResetSpawners();
        _gameMediator.ResetScore();

        IPlatform platform = await CreatePlatform();
        IHero hero = CreateHero(platform);

        stateMachine.Enter<MoveHeroToPlatformState, MoveHeroToPlatformState.Arguments>(
            new(platform, platform, hero, StickNull.Default, CherryNull.Default));
    }

    private async UniTask ChangeBackground()
    {
        await _camera.ChangeBackgroundAsync();
    }

    private void ResetCamera()
    {
        _camera.RestorePositionToInitial();
    }

    private void ResetSpawners()
    {
        _platformSpawner.DespawnAll();
        _cherrySpawner.DespawnAll();
        _stickSpawner.DespawnAll();
    }

    private async UniTask<IPlatform> CreatePlatform()
    {
        return await _platformSpawner.CreatePlatformAsync(_camera.Borders.Left, Relative.Left, false);
    }

    private IHero CreateHero(IPlatform platform)
    {
        return _heroSpawner.Create(platform.Borders.LeftTop, Relative.Center);
    }
}
