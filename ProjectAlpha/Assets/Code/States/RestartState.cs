using Code.Services;
using Cysharp.Threading.Tasks;

namespace Code.States;

public sealed class RestartState : IState
{
    private readonly PlatformSpawner _platformSpawner;
    private readonly CherrySpawner _cherrySpawner;
    private readonly StickSpawner _stickSpawner;
    private readonly Camera _camera;
    private readonly HeroSpawner _heroSpawner;

    public RestartState(
        PlatformSpawner platformSpawner,
        CherrySpawner cherrySpawner,
        StickSpawner stickSpawner,
        Camera camera,
        HeroSpawner heroSpawner)
    {
        _platformSpawner = platformSpawner;
        _cherrySpawner = cherrySpawner;
        _stickSpawner = stickSpawner;
        _camera = camera;
        _heroSpawner = heroSpawner;
    }

    public async UniTaskVoid EnterAsync(IStateMachine stateMachine)
    {
        await _camera.ChangeBackgroundAsync();

        _platformSpawner.DespawnAll();
        _cherrySpawner.DespawnAll();
        _stickSpawner.DespawnAll();

        IPlatform platform =
            await _platformSpawner.CreateAsync(_camera.Borders.Left, Relative.Left, false);

        IHero hero = await _heroSpawner.CreateAsync(platform.Borders.LeftTop, Relative.Center);

        stateMachine.Enter<GameStartState, GameStartState.Arguments>(new(platform, hero));
    }

    public void Exit() { }
}