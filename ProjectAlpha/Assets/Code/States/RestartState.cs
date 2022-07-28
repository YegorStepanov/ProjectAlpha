using Code.Services;
using Code.Services.Game.UI;
using Cysharp.Threading.Tasks;

namespace Code.States;

public sealed class RestartState : IState
{
    private readonly GameWorld _gameWorld;
    private readonly GameStateResetter _gameStateResetter;
    private readonly PlatformSpawner _platformSpawner;
    private readonly Camera _camera;
    private readonly HeroSpawner _heroSpawner;

    public RestartState(GameWorld gameWorld,
        GameStateResetter gameStateResetter, PlatformSpawner platformSpawner, Camera camera, HeroSpawner heroSpawner)
    {
        _gameWorld = gameWorld;
        _gameStateResetter = gameStateResetter;
        _platformSpawner = platformSpawner;
        _camera = camera;
        _heroSpawner = heroSpawner;
    }

    public async UniTaskVoid EnterAsync(IGameStateMachine stateMachine)
    {
        SwitchToGameHeight();
        await ResetGameState();

        IPlatform platform = await CreatePlatform();
        IHero hero = CreateHero(platform);

        stateMachine.Enter<MoveHeroToPlatformState, MoveHeroToPlatformState.Arguments>(
            new(platform, platform, hero, StickNull.Default, CherryNull.Default));
    }

    private UniTask ResetGameState() =>
        _gameStateResetter.ResetAsync();

    private void SwitchToGameHeight() =>
        _gameWorld.SwitchToGameHeight();

    private UniTask<IPlatform> CreatePlatform() =>
        _platformSpawner.CreatePlatformAsync(_camera.Borders.Left, Relative.Left, false);

    private IHero CreateHero(IPlatform platform) =>
        _heroSpawner.Create(platform.Borders.LeftTop, Relative.Center);
}
