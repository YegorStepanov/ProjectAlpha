using Code.Services;
using Code.Services.Data;
using Code.Services.UI;
using Cysharp.Threading.Tasks;

namespace Code.States
{
    public sealed class HeroMovementToPlatformState : IState<GameData>
    {
        private readonly HeroMovement _heroMovement;
        private readonly GameUIController _gameUIController;
        private readonly IProgress _progress;
        private readonly GameSettings _settings;

        public HeroMovementToPlatformState(HeroMovement heroMovement, GameUIController gameUIController, IProgress progress, GameSettings settings)
        {
            _heroMovement = heroMovement;
            _gameUIController = gameUIController;
            _progress = progress;
            _settings = settings;
        }

        public async UniTaskVoid EnterAsync(GameData data, IGameStateMachine stateMachine)
        {
            var result = await _heroMovement.MoveHeroToNextPlatformAsync(data, false);
            await UniTask.Delay(_settings.DelayAfterHeroMovementToPlatform);

            if (result.IsHeroCollided)
            {
                stateMachine.Enter<EndGameState, GameData>(data);
                return;
            }

            //collect cherry only if successful
            if (result.IsCherryCollected)
                CollectCherry();

            IncreaseScore();
            _gameUIController.HideHelp();

            stateMachine.Enter<NextRoundState, GameData>(data);
        }

        private void CollectCherry() =>
            _progress.Persistant.AddCherries(1);

        private void IncreaseScore() =>
            _progress.Session.IncreaseScore();
    }
}