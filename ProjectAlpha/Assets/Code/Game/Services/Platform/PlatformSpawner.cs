using Code.Common;
using Code.Extensions;
using UnityEngine;

namespace Code.Game
{
    public sealed class PlatformSpawner
    {
        // private const float gamePlatformViewportPosX = 1f;

        private readonly PlatformController.Pool pool;
        private readonly CameraService cameraService;
        // private readonly WidthGenerator widthGenerator;
        // private readonly Settings settings;

        // [Serializable]
        // public class Settings
        // {
        //     public float MenuPlatformWidth = 2f;
        //
        //     public float MenuPlatformViewportPosX = 0.5f; //= new(0.5f, 0.2f);
        //     // public float GamePlatformViewportPosX = 1f; //= new(1f, 0.3f); //y should be synced with camera
        //
        //     public float MenuViewportPosY = 0.2f;
        //     public float GameViewportPosY = 0.3f;
        // }

        public PlatformSpawner(
            PlatformController.Pool pool,
            CameraService cameraService
            // GameSettings gameSettings, 
            // WidthGenerator widthGenerator
            // Settings settings
        )
        {
            this.pool = pool;
            this.cameraService = cameraService;
            // this.widthGenerator = widthGenerator;
            // settings = new Settings();
        }

        // public IPlatformController CreateMenuPlatform() =>
        //     CreatePlatform(new Vector2(
        //             settings.MenuPlatformViewportPosX,
        //             settings.MenuViewportPosY),
        //         settings.MenuPlatformWidth,
        //         Relative.Center);

        // public IPlatformController CreateMenuGamePlatform(float posX, Relative relative) =>
        //     CreatePlatform(
        //         new Vector2(posX, settings.MenuViewportPosY),
        //         widthGenerator.NextWidth(),
        //         relative);
        //

        // public IPlatformController CreateGamePlatform() =>
        //     CreatePlatform(
        //         new Vector2(gamePlatformViewportPosX, settings.GameViewportPosY),
        //         widthGenerator.NextWidth(),
        //         Relative.Left);

        public IPlatformController CreatePlatform(Vector2 position, float width, Relative relative)
        {
            PlatformController platform = pool.Spawn();

            // Vector2 position = ViewportToWorldPosition(viewportPosition);
            // position.x = viewportPosition.x; //TODO

            float height = cameraService.Borders.TransformPointY(position.y, Relative.Bottom);

            Vector2 size = new(width, height);
            platform.SetSize(size);

            position = platform.Borders.TransformPoint(position, relative);
            platform.SetPosition(position);
            
            return platform;
        }

        private Vector2 ViewportToWorldPosition(Vector2 viewportPosition) =>
            cameraService.ViewportToWorldPosition(viewportPosition);
    }
}