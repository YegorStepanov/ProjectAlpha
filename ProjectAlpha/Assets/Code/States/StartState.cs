using Code.Common;
using Code.Services;
using Code.Services.Entities;
using Code.Services.Spawners;
using Code.Services.UI;
using Cysharp.Threading.Tasks;
using MessagePipe;

namespace Code.States
{
    public sealed class StartState : IState
    {
        private readonly ICameraResetter _cameraResetter;
        private readonly GameUIController _gameUIController;
        private readonly HeroSpawner _heroSpawner;
        private readonly PlatformSpawner _platformSpawner;
        private readonly GameStateResetter _gameStateResetter;
        private readonly GameStartEventAwaiter _gameStartEventAwaiter;
        private readonly GameHeightFactory _gameHeightFactory;
        private readonly IPublisher<Event.GameSceneLoaded> _gameSceneLoadedEvent;

        public StartState(
            ICameraResetter cameraResetter,
            GameUIController gameUIController,
            HeroSpawner heroSpawner,
            PlatformSpawner platformSpawner,
            GameStateResetter gameStateResetter,
            GameStartEventAwaiter gameStartEventAwaiter,
            GameHeightFactory gameHeightFactory,
            IPublisher<Event.GameSceneLoaded> gameSceneLoadedEvent)
        {
            _cameraResetter = cameraResetter;
            _gameUIController = gameUIController;
            _heroSpawner = heroSpawner;
            _platformSpawner = platformSpawner;
            _gameStateResetter = gameStateResetter;
            _gameStartEventAwaiter = gameStartEventAwaiter;
            _gameHeightFactory = gameHeightFactory;
            _gameSceneLoadedEvent = gameSceneLoadedEvent;
        }

        public async UniTaskVoid EnterAsync(IGameStateMachine stateMachine)
        {
            _gameStateResetter.ResetState();
            _cameraResetter.ResetPosition();
            _gameUIController.HideUI();

            GameHeight gameHeight = _gameHeightFactory.CreateStartHeight();

            (IHero hero, IPlatform platform) = await (
                _heroSpawner.CreateAsync(default, default),
                _platformSpawner.CreateMenuPlatformAsync(gameHeight.PositionY, gameHeight.Height));

            hero.SetPosition(platform.Borders.CenterTop, Relative.Bot);

            _gameSceneLoadedEvent.Publish(new());

            await _gameStartEventAwaiter.Wait();
            _gameUIController.ShowUI();

            stateMachine.Enter<HeroMovementToStartPlatformState, GameData>(
                new GameData(hero, platform, platform, CherryNull.Default, StickNull.Default, gameHeight));
        }
    }
}