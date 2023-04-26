using Code.Common;
using Code.Services;
using Code.Services.Entities;
using Code.Services.Spawners;
using Code.Services.UI;
using Cysharp.Threading.Tasks;

namespace Code.States
{
    public sealed class StartState : IState
    {
        private readonly ICameraRestorer _cameraRestorer;
        private readonly GameUIController _gameUIController;
        private readonly HeroSpawner _heroSpawner;
        private readonly PlatformSpawner _platformSpawner;
        private readonly GameStateResetter _gameStateResetter;
        private readonly GameStartEventAwaiter _gameStartEventAwaiter;
        private readonly GameHeightFactory _gameHeightFactory;

        public StartState(
            ICameraRestorer cameraRestorer,
            GameUIController gameUIController,
            HeroSpawner heroSpawner,
            PlatformSpawner platformSpawner,
            GameStateResetter gameStateResetter,
            GameStartEventAwaiter gameStartEventAwaiter,
            GameHeightFactory gameHeightFactory)
        {
            _cameraRestorer = cameraRestorer;
            _gameUIController = gameUIController;
            _heroSpawner = heroSpawner;
            _platformSpawner = platformSpawner;
            _gameStateResetter = gameStateResetter;
            _gameStartEventAwaiter = gameStartEventAwaiter;
            _gameHeightFactory = gameHeightFactory;
        }

        public async UniTaskVoid EnterAsync(IGameStateMachine stateMachine)
        {
            _cameraRestorer.RestorePosition();
            _gameUIController.HideGameOver();
            _gameUIController.HideScore();

            _gameStateResetter.ResetState();

            GameHeight gameHeight = _gameHeightFactory.CreateStartHeight();
            IPlatform platform = await _platformSpawner.CreateMenuPlatformAsync(gameHeight.PositionY, gameHeight.Height);
            IHero hero = await _heroSpawner.CreateAsync(platform.Borders.CenterTop, Relative.Bot);

            await _gameStartEventAwaiter.Wait();
            _gameUIController.ShowUI();

            stateMachine.Enter<HeroMovementToStartPlatformState, GameData>(
                new GameData(hero, platform, platform, CherryNull.Default, StickNull.Default, gameHeight));
        }
    }
}
