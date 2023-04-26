using Code.Data;
using Code.Services;
using Code.Services.Entities;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.States
{
    public sealed class CameraMovementState : IState<(GameData Data, Vector2 CameraDestination)>
    {
        private readonly ICamera _camera;
        private readonly PlatformPositionGenerator _platformPositionGenerator;
        private readonly CherryPositionGenerator _cherryPositionGenerator;

        public CameraMovementState(
            ICamera camera,
            PlatformPositionGenerator platformPositionGenerator,
            CherryPositionGenerator cherryPositionGenerator)
        {
            _camera = camera;
            _platformPositionGenerator = platformPositionGenerator;
            _cherryPositionGenerator = cherryPositionGenerator;
        }

        public async UniTaskVoid EnterAsync((GameData Data, Vector2 CameraDestination) args, IGameStateMachine stateMachine)
        {
            (GameData data, Vector2 cameraDestination) = args;

            data.CurrentPlatform.PlatformRedPoint.FadeOutAsync().Forget();

            await UniTask.WhenAll(
                MoveCamera(cameraDestination),
                MovePlatformWithCherry(cameraDestination, data.CurrentPlatform, data.NextPlatform, data.Cherry));

            stateMachine.Enter<EntitiesMovementState, GameData>(data);
        }

        private UniTask MoveCamera(Vector2 cameraDestination) =>
            _camera.MoveAsync(cameraDestination);

        private async UniTask MovePlatformWithCherry(Vector2 cameraDestination, IPlatform currentPlatform, IPlatform nextPlatform, ICherry nextCherry)
        {
            float nextRightBorder = GetNextRightBorder(cameraDestination);

            float nextPlatformPosX = GetNextPlatformPosX(
                currentPlatform.Borders.Right, nextRightBorder, nextPlatform.Borders.Width);

            float cherryDestinationX = GetNextCherryPosX(
                currentPlatform.Borders.Right, nextPlatformPosX - nextPlatform.Borders.HalfWidth, nextCherry.Borders.Width);

            await UniTask.WhenAll(
                nextPlatform.MoveAsync(nextPlatformPosX),
                nextCherry.MoveAsync(cherryDestinationX));
        }

        private float GetNextRightBorder(Vector2 cameraDestination) =>
            cameraDestination.x + _camera.Borders.HalfWidth;

        private float GetNextPlatformPosX(float minPosX, float maxPosX, float width) =>
            _platformPositionGenerator.NextPosition(minPosX, maxPosX, width);

        private float GetNextCherryPosX(float minPosX, float maxPosX, float width) =>
            _cherryPositionGenerator.NextPosition(minPosX, maxPosX, width);
    }
}
