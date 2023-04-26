using Code.Services;
using Code.Services.Entities;
using Code.Services.UI;
using Cysharp.Threading.Tasks;

namespace Code.States
{
    public sealed class EndGameState : IState<GameData>
    {
        private readonly ICamera _camera;
        private readonly GameUIController _gameUIController;
        private readonly GameSettings _settings;

        public EndGameState(
            ICamera camera,
            GameUIController gameUIController,
            GameSettings settings)
        {
            _camera = camera;
            _gameUIController = gameUIController;
            _settings = settings;
        }

        public async UniTaskVoid EnterAsync(GameData data, IGameStateMachine stateMachine)
        {
            await Delay(_settings.DelayBeforeRotatingStickDownInEndGame);
            RotateStickDown(data.Stick);
            await Delay(_settings.DelayBeforeFallingInEndGame);
            await FallHero(data.Hero);
            await Delay(_settings.DelayBeforeEndGameInEndGame);
            await _camera.PunchAsync();
            _gameUIController.ShowGameOver();
        }

        private static void RotateStickDown(IStick stick) =>
            stick.RotateDownAsync().Forget();

        private UniTask FallHero(IHero hero) =>
            hero.FallAsync(_camera.Borders.Bot - hero.Borders.Height);

        private static UniTask Delay(int delay) =>
            UniTask.Delay(delay);
    }
}
