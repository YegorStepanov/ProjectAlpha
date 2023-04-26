using Code.Services;
using Cysharp.Threading.Tasks;

namespace Code.States
{
    public sealed class HeroMovementToStartPlatformState : IState<GameData>
    {
        private readonly HeroMovement _heroMovement;

        public HeroMovementToStartPlatformState(HeroMovement heroMovement) =>
            _heroMovement = heroMovement;

        public async UniTaskVoid EnterAsync(GameData data, IGameStateMachine stateMachine)
        {
            await _heroMovement.MoveHeroToNextPlatformAsync(data, false);

            stateMachine.Enter<NextRoundState, GameData>(data);
        }
    }
}
