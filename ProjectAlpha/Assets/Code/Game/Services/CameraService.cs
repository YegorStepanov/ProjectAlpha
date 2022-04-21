using System;
using Code.Common;
using Code.Extensions;
using Code.Project;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Code.Game
{
    public sealed class CameraService : IInitializable, IDisposable //: ITickable
    {
        private readonly Camera camera;
        private readonly GameSettings gameSettings;
        private readonly ScreenSizeChecker screenSizeChecker;

        public Borders Borders => UpdateBorders();

        public Vector3 CameraPosition => camera.transform.position;

        public CameraService(Camera camera, GameSettings gameSettings, ScreenSizeChecker screenSizeChecker)
        {
            this.camera = camera;
            this.gameSettings = gameSettings;
            this.screenSizeChecker = screenSizeChecker;

            // screenSizeChecker.OnScreenResized += UpdateBorders;
            Debug.Log("CameraService.Ctor");
        }

        void IInitializable.Initialize()
        {
            Debug.Log("CameraService.Initialize" + ": " + Time.frameCount);
            // UpdateBorders(screenSizeChecker.ScreenSize);
        }

        void IDisposable.Dispose()
        {
            // screenSizeChecker.OnScreenResized -= UpdateBorders;
        }

        public async UniTask ShiftAsync(Vector2 offset) =>
            await MoveAsync(CameraPosition.ShiftXY(-offset));


        public async UniTask MoveAsync(float destinationX, Relative relative = Relative.Center)
        {
            float finalX = Borders.TransformPointX(destinationX, relative);
            await MoveAsync(CameraPosition.WithX(finalX));
        }

        public async UniTask MoveAsync(Vector2 destination, Relative relative = Relative.Center)
        {
            Vector2 finalDestination = GetRelativePosition(destination, relative);
            await MoveAsync(CameraPosition.WithXY(finalDestination));
        }
        
        public Vector2 GetRelativePosition(Vector2 position, Relative relative) => 
            Borders.TransformPoint(position, relative);

        private async UniTask MoveAsync(Vector3 destination) =>
            await camera.transform.DOMove(destination, 0.3f);


        private void Move(float cameraDestX) //Transition from the Menu to the Game state
        {
            throw new NotImplementedException();
            float offsetX = OffsetToLeftCameraBorder() + MenuPlatformOffsetToLeftBorder();
            float offsetY = MenuToGamePlatformOffsetY();

            var offset = new Vector3(offsetX, offsetY, camera.transform.position.z);

            camera.transform.DOMove(offset, gameSettings.MenuToGameCameraAnimationDuration);
        }

        private Borders UpdateBorders()
        {
            Debug.Log("CameraService.UpdateBorders" + ": " + Time.frameCount);

            Vector2 topRightCorner = camera.ViewportToWorldPoint(Vector2.one);
            Vector2 bottomLeftCorner = camera.ViewportToWorldPoint(Vector2.zero);

            return new Borders(
                Top: topRightCorner.y,
                Right: topRightCorner.x,
                Bottom: bottomLeftCorner.y,
                Left: bottomLeftCorner.x
            );
        }

        public Vector2 ViewportToWorldPosition(Vector2 viewportPosition) =>
            camera.ViewportToWorldPoint(viewportPosition);

        // public float WorldHeight() =>
        //     camera.orthographicSize * 2;
        //
        // public float WorldWidth() =>
        //     camera.orthographicSize * camera.aspect * 2;

        private float OffsetToLeftCameraBorder() =>
            -ViewportToWorldPosition(Vector2.zero).x;

        private float MenuPlatformOffsetToLeftBorder() => 0f;
        // -gameSettings.MenuPlatformWidth / 2f;

        private float MenuToGamePlatformOffsetY() => 0f;
        // gameSettings.GamePlatformPositionY - gameSettings.MenuPlatformPositionY;
    }
}