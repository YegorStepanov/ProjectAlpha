using Cysharp.Threading.Tasks;

namespace Code.Game.States
{
    public sealed class MoveHeroToNextPlatformState : IArgState<MoveHeroToNextPlatformState.Arguments>
    {
        private readonly GameStateMachine stateMachine;
        private readonly IHeroController hero;
        private readonly CameraService cameraService;
        private readonly PlatformSpawner platformSpawner;

        public sealed record Arguments(IPlatformController CurrentPlatform, IPlatformController NextPlatform, IStickController Stick);

        public MoveHeroToNextPlatformState(
            GameStateMachine stateMachine, 
            IHeroController hero,
            CameraService cameraService,
            PlatformSpawner platformSpawner)
        {
            this.stateMachine = stateMachine;
            this.hero = hero;
            this.cameraService = cameraService;
            this.platformSpawner = platformSpawner;
        }

        public async UniTaskVoid EnterAsync(Arguments args)
        {
            if (IsStickOnPlatform(args.Stick, args.NextPlatform))
            {
                stateMachine.Enter<GameStartState, GameStartState.Arguments>(
                    new GameStartState.Arguments(args.NextPlatform));
                
                
                // await hero.MoveAsync(args.NextPlatform.Borders.Right);
                //
                // await cameraService.MoveAsync(args.NextPlatform.Borders.Left, Relative.Left);
                //
                // var newPlatform = platformSpawner.CreateGamePlatform();
                //
                // stateMachine.Enter<StickControlState, StickControlState.Arguments>(
                //     new StickControlState.Arguments(args.NextPlatform, newPlatform));
            }
            else
            {
                await hero.MoveWithoutStoppingAsync(args.Stick.Borders.Right);
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
        
        private async UniTask MoveHeroAsync(IHeroController hero, IPlatformController platform, float stickWidth)
        {
            float destination = platform.Borders.Right;
            float offset = stickWidth / 2f + 0.25f;
            // await hero.MoveToAsync(destination - offset);
        }
    }
}