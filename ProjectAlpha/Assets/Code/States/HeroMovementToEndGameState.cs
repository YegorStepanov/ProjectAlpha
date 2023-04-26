using Code.Services;
using Cysharp.Threading.Tasks;

namespace Code.States
{
    public sealed class HeroMovementToEndGameState : IState<GameData>
    {
        private readonly HeroMovement _heroMovement;

        public HeroMovementToEndGameState(HeroMovement heroMovement) =>
            _heroMovement = heroMovement;

        public async UniTaskVoid EnterAsync(GameData data, IGameStateMachine stateMachine)
        {
            await _heroMovement.MoveHeroToStickEndAsync(data, canHeroFlipsOnNextPlatform: true);

            stateMachine.Enter<EndGameState, GameData>(data);
        }
    }
}
