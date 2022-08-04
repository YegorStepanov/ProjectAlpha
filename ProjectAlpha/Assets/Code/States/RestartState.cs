using Code.Common;
using Code.Services;
using Code.Services.Entities;
using Code.Services.Infrastructure;
using Code.Services.Spawners;
using Code.Services.UI;
using Cysharp.Threading.Tasks;

namespace Code.States;

public sealed class RestartState : IState
{
    private readonly GameWorld _gameWorld;
    private readonly GameStateResetter _gameStateResetter;
    private readonly PlatformSpawner _platformSpawner;
    private readonly ICamera _camera1;
    private readonly HeroSpawner _heroSpawner;
    private readonly GameUIController _gameUIController;

    public RestartState(GameWorld gameWorld,
        GameStateResetter gameStateResetter, PlatformSpawner platformSpawner, ICamera camera1, HeroSpawner heroSpawner, GameUIController gameUIController)
    {
        _gameWorld = gameWorld;
        _gameStateResetter = gameStateResetter;
        _platformSpawner = platformSpawner;
        _camera1 = camera1;
        _heroSpawner = heroSpawner;
        _gameUIController = gameUIController;
    }

    public async UniTaskVoid EnterAsync(IGameStateMachine stateMachine)
    {
        _gameUIController.HideGameOver();

        SwitchToGameHeight();
        ResetGameState();

        IPlatform platform = await CreatePlatform();
        IHero hero = CreateHero(platform);

        stateMachine.Enter<MoveHeroToPlatformState, MoveHeroToPlatformState.Arguments>(
            new(platform, platform, hero, StickNull.Default, CherryNull.Default));
    }

    private void ResetGameState() =>
        _gameStateResetter.ResetState();

    private void SwitchToGameHeight() =>
        _gameWorld.SwitchToGameHeight();

    private UniTask<IPlatform> CreatePlatform() =>
        _platformSpawner.CreatePlatformAsync(_camera1.Borders.Left, Relative.Left, false);

    private IHero CreateHero(IPlatform platform) =>
        _heroSpawner.Create(platform.Borders.LeftTop, Relative.Center);
}
