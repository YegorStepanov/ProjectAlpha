using System;
using System.Collections.Generic;
using Code.Extensions;
using Code.Game;
using UnityEngine;

namespace Code.Game
{
    public sealed class PlatformSpawner
    {
        private readonly PlatformController.Pool pool;
        private readonly CameraService cameraService;
        private readonly PlatformWidthGenerator widthGenerator;
        private readonly Settings settings;
        
        [Serializable]
        public class Settings
        {
            public float MenuPlatformWidth = 2f;

            public Vector2 MenuPlatformViewportPosition = new(0.5f, 0.2f);
            public Vector2 GamePlatformViewportPosition = new(1f, 0.3f); //y should be synced with camera
        }

        public PlatformSpawner(
            PlatformController.Pool pool, 
            CameraService cameraService, 
            // GameSettings gameSettings, 
            PlatformWidthGenerator widthGenerator
            // Settings settings
            )
        {
            this.pool = pool;
            this.cameraService = cameraService;
            this.widthGenerator = widthGenerator;
            settings = new Settings();
        }

        public IPlatformController CreateMenuPlatform() =>
            CreatePlatform(
                settings.MenuPlatformViewportPosition,
                settings.MenuPlatformWidth);

        public IPlatformController CreatePlatform() =>
            CreatePlatform(
                settings.GamePlatformViewportPosition,
                widthGenerator.NextWidth());

        private IPlatformController CreatePlatform(Vector2 viewportPosition, float width, float extraHeight = 0f)
        {
            PlatformController platform = pool.Spawn();
            
            Vector2 position = ViewportToWorld(viewportPosition);
            platform.SetPosition(position);
            
            //pos - cam_offset - bottom
            
            float cameraGameOffsetY =
                position.y - ViewportToWorld(cameraService.ViewportGameCameraOffset).y;
            
            float height = Mathf.Abs(cameraGameOffsetY - cameraService.GameBorders.Bottom);

            Vector2 size = new(width, height);
            platform.SetSize(size);
            
            return platform;
        }

        private Vector2 ViewportToWorld(Vector2 position) =>
            cameraService.ViewportToWorldPosition(position);
    }
}