using Code.Common;
using Code.Services;
using Code.Services.Entities;
using Code.Services.Spawners;
using Code.Services.UI;
using Cysharp.Threading.Tasks;
using MessagePipe;

namespace Code.States
{
    public sealed class RestartState : IState
    {
        private readonly ICameraResetter _cameraResetter;
        private readonly CameraBackground _cameraBackground;
        private readonly GameStateResetter _gameStateResetter;
        private readonly HeroSpawner _heroSpawner;
        private readonly GameUIController _gameUIController;
        private readonly GameHeightFactory _gameHeightFactory;
        private readonly PlatformSpawner _platformSpawner;
        private readonly IPublisher<Event.GameSceneLoaded> _gameSceneLoadedEvent;

        public RestartState(
            ICameraResetter cameraResetter,
            CameraBackground cameraBackground,
            GameStateResetter gameStateResetter,
            HeroSpawner heroSpawner,
            GameUIController gameUIController,
            GameHeightFactory gameHeightFactory,
            PlatformSpawner platformSpawner,
            IPublisher<Event.GameSceneLoaded> gameSceneLoadedEvent)
        {
            _cameraResetter = cameraResetter;
            _cameraBackground = cameraBackground;
            _gameStateResetter = gameStateResetter;
            _heroSpawner = heroSpawner;
            _gameUIController = gameUIController;
            _gameHeightFactory = gameHeightFactory;
            _platformSpawner = platformSpawner;
            _gameSceneLoadedEvent = gameSceneLoadedEvent;
        }

        public async UniTaskVoid EnterAsync(IGameStateMachine stateMachine)
        {
            await _cameraBackground.ChangeBackgroundAsync();
            _gameStateResetter.ResetState();
            _cameraResetter.ResetPositionX();
            _gameUIController.HideGameOver();

            GameHeight gameHeight = _gameHeightFactory.CreateRestartHeight();

            (IHero hero, IPlatform platform) = await (
                _heroSpawner.CreateAsync(default, default),
                _platformSpawner.CreateRestartPlatformAsync(gameHeight.PositionY, gameHeight.Height));

            hero.SetPosition(platform.Borders.LeftTop, Relative.Bot);

            _gameSceneLoadedEvent.Publish(new());

            stateMachine.Enter<HeroMovementToPlatformState, GameData>(
                new GameData(hero, platform, platform, CherryNull.Default, StickNull.Default, gameHeight));
        }
    }
}