using Code.Services;
using Cysharp.Threading.Tasks;

namespace Code.States
{
    public sealed class MoveHeroToNextPlatformState : IArgState<MoveHeroToNextPlatformState.Arguments>
    {
        private readonly GameStateMachine stateMachine;
        private readonly IHeroController hero;
        private readonly PlatformSpawner platformSpawner;

        public sealed record Arguments(IPlatformController CurrentPlatform, IPlatformController NextPlatform,
            IStickController Stick);

        public MoveHeroToNextPlatformState(
            GameStateMachine stateMachine,
            IHeroController hero,
            PlatformSpawner platformSpawner)
        {
            this.stateMachine = stateMachine;
            this.hero = hero;
            this.platformSpawner = platformSpawner;
        }

        public async UniTaskVoid EnterAsync(Arguments args)
        {
            if (IsStickOnPlatform(args.Stick, args.NextPlatform))
            {
                stateMachine.Enter<GameStartState, GameStartState.Arguments>(
                    new GameStartState.Arguments(args.NextPlatform));
            }
            else
            {
                await hero.MoveAsync(args.Stick.Borders.Right);
                await hero.FellAsync();

                //earthshake screen
            }
        }

        public void Exit() { }

        private static bool IsStickOnPlatform(IStickController stick, IPlatformController platform)
        {
            var stickPosX = stick.Borders.Right;

            if (stickPosX < platform.Borders.Left)
                return false;

            if (stickPosX > platform.Borders.Right)
                return false;

            return true;
        }
    }
}