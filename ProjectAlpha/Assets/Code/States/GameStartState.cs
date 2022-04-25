using Code.Services;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.States
{
    public sealed class GameStartState : IArgState<GameStartState.Arguments>
    {
        public sealed record Arguments(IPlatformController CurrentPlatform);

        private readonly GameStateMachine stateMachine;
        private readonly CameraService cameraService;
        private readonly IHeroController hero;
        private readonly PlatformSpawner platformSpawner;
        private readonly StickSpawner stickSpawner;
        private readonly WidthGenerator widthGenerator;

        public GameStartState(
            GameStateMachine stateMachine,
            CameraService cameraService,
            IHeroController hero,
            PlatformSpawner platformSpawner,
            StickSpawner stickSpawner,
            WidthGenerator widthGenerator)
        {
            this.stateMachine = stateMachine;
            this.cameraService = cameraService;
            this.hero = hero;
            this.platformSpawner = platformSpawner;
            this.stickSpawner = stickSpawner;
            this.widthGenerator = widthGenerator;
        }

        public async UniTaskVoid EnterAsync(Arguments args)
        {
            await MoveHeroAsync(args.CurrentPlatform);

            await UniTask.Delay(100);
            UniTask moveCameraTask = MoveCameraAsync(args.CurrentPlatform);

            IPlatformController nextPlatform = CreateNextPlatform(args.CurrentPlatform);
            UniTask movePlatformTask = MoveNextPlatformToRandomPoint(args.CurrentPlatform, nextPlatform);

            await (moveCameraTask, movePlatformTask);

            stateMachine.Enter<StickControlState, StickControlState.Arguments>(
                new StickControlState.Arguments(args.CurrentPlatform, nextPlatform));
        }

        private async UniTask MoveCameraAsync(IPlatformController currentPlatform)
        {
            Vector2 destination = new(currentPlatform.Borders.Left, currentPlatform.Borders.Bottom);
            await cameraService.MoveAsync(destination, Relative.LeftBottom);
        }

        private async UniTask MoveHeroAsync(IPlatformController currentPlatform)
        {
            float destX = currentPlatform.Borders.Right;
            destX -= stickSpawner.StickWidth / 2f;
            destX -= hero.HandOffset;
            await UniTask.Delay(200);
            await hero.MoveAsync(destX);
        }

        private IPlatformController CreateNextPlatform(IPlatformController currentPlatform)
        {
            float leftCameraBorderToPlatformDistance = currentPlatform.Borders.Left - cameraService.Borders.Left;
            Vector2 position = new(
                cameraService.Borders.Right + leftCameraBorderToPlatformDistance,
                currentPlatform.Borders.Top);

            return platformSpawner.CreatePlatform(position, widthGenerator.NextWidth(), Relative.Left);
        }

        private static async UniTask MoveNextPlatformToRandomPoint(
            IPlatformController currentPlatform,
            IPlatformController nextPlatform)
        {
            float halfWidth = nextPlatform.Borders.Width / 2f;
            const float minDistance = 0.5f; //minOffset
            float posX = Random.Range(currentPlatform.Borders.Right + halfWidth + minDistance,
                nextPlatform.Borders.Left - halfWidth);

            int randDelay = Random.Range(0, 300); //ms
            await UniTask.Delay(randDelay);

            await nextPlatform.MoveAsync(posX);
        }

        public void Exit() { }
    }

    public sealed class StickBuildingState
    {
        //hero is присидает каждый тик
        //палка увеличивается линейно

        //next state
        //бьет ногой
        //палка поворачивается
        //

        //чел бежит к концу следующей платформы
        //задний фон плывет

        //пододвинуть камеру к началу новой платформы
    }
}