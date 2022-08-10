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
    private readonly ICameraRestorer _cameraRestorer;
    private readonly CameraBackground _cameraBackground;
    private readonly GameStateResetter _gameStateResetter;
    private readonly HeroSpawner _heroSpawner;
    private readonly GameUIController _gameUIController;
    private readonly GameHeightFactory _gameHeightFactory;
    private readonly PlatformSpawner _platformSpawner;

    public RestartState(
        ICameraRestorer cameraRestorer,
        CameraBackground cameraBackground,
        GameStateResetter gameStateResetter,
        HeroSpawner heroSpawner,
        GameUIController gameUIController,
        GameHeightFactory gameHeightFactory,
        PlatformSpawner platformSpawner)
    {
        _cameraRestorer = cameraRestorer;
        _cameraBackground = cameraBackground;
        _gameStateResetter = gameStateResetter;
        _heroSpawner = heroSpawner;
        _gameUIController = gameUIController;
        _gameHeightFactory = gameHeightFactory;
        _platformSpawner = platformSpawner;
    }

    public async UniTaskVoid EnterAsync(IGameStateMachine stateMachine)
    {
        await _cameraBackground.ChangeBackgroundAsync();
        _cameraRestorer.RestorePositionX();
        _gameUIController.HideGameOver();
        _gameStateResetter.ResetState();

        GameHeight gameHeight = _gameHeightFactory.CreateRestartHeight();
        IPlatform platform = await _platformSpawner.CreateRestartPlatformAsync(gameHeight.PositionY, gameHeight.Height);
        IHero hero = await _heroSpawner.CreateAsync(platform.Borders.LeftTop, Relative.Bot);

        stateMachine.Enter<HeroMovementToPlatformState, GameData>(
            new GameData(hero, platform, platform, CherryNull.Default, StickNull.Default, gameHeight));
    }
}
