using Code.Common;
using Code.Services;
using Code.Services.Entities.Cherry;
using Code.Services.Entities.Hero;
using Code.Services.Entities.Platform;
using Code.Services.Entities.Stick;
using Code.Services.Infrastructure;
using Code.Services.Spawners;
using Cysharp.Threading.Tasks;

namespace Code.States;

public sealed class RestartState : IState
{
    private readonly GameWorld _gameWorld;
    private readonly GameStateResetter _gameStateResetter;
    private readonly PlatformSpawner _platformSpawner;
    private readonly ICamera _camera1;
    private readonly HeroSpawner _heroSpawner;

    public RestartState(GameWorld gameWorld,
        GameStateResetter gameStateResetter, PlatformSpawner platformSpawner, ICamera camera1, HeroSpawner heroSpawner)
    {
        _gameWorld = gameWorld;
        _gameStateResetter = gameStateResetter;
        _platformSpawner = platformSpawner;
        _camera1 = camera1;
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
        _platformSpawner.CreatePlatformAsync(_camera1.Borders.Left, Relative.Left, false);

    private IHero CreateHero(IPlatform platform) =>
        _heroSpawner.Create(platform.Borders.LeftTop, Relative.Center);
}
