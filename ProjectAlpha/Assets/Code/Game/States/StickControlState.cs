using Cysharp.Threading.Tasks;

namespace Code.Game.States
{
    public sealed class StickControlState : IArgState<StickControlState.Arguments>
    {
        public sealed record Arguments(IPlatformController CurrentPlatform, IPlatformController NextPlatform);

        private readonly GameStateMachine stateMachine;
        private readonly StickSpawner stickSpawner;
        private readonly InputManager inputManager;

        public StickControlState(GameStateMachine stateMachine, StickSpawner stickSpawner, InputManager inputManager)
        {
            this.stateMachine = stateMachine;
            this.stickSpawner = stickSpawner;
            this.inputManager = inputManager;
        }

        public async UniTaskVoid EnterAsync(Arguments args)
        {
            float positionX = args.CurrentPlatform.Borders.Right;
            IStickController stick = stickSpawner.Spawn(positionX);

            await inputManager.NextMouseClick();
            stick.StartIncreasing();
            
            await inputManager.NextMouseRelease();
            stick.StopIncreasing();
            
            await stick.RotateAsync();
            
            stateMachine.Enter<MoveHeroToNextPlatformState, MoveHeroToNextPlatformState.Arguments>(
                new MoveHeroToNextPlatformState.Arguments(args.CurrentPlatform, args.NextPlatform, stick));
        }

        public void Exit() { }
    }
}