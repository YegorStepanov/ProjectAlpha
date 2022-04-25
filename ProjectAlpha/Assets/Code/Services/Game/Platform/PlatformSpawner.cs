using UnityEngine;

namespace Code.Services
{
    public sealed class PlatformSpawner
    {
        private readonly PlatformController.Pool pool;
        private readonly CameraService cameraService;

        public PlatformSpawner(PlatformController.Pool pool, CameraService cameraService)
        {
            this.pool = pool;
            this.cameraService = cameraService;
        }

        public IPlatformController CreatePlatform(Vector2 position, float width, Relative relative)
        {
            PlatformController platform = pool.Spawn();

            float height = cameraService.Borders.TransformPointY(position.y, Relative.Bottom);

            Vector2 size = new(width, height);
            platform.SetSize(size);

            position = platform.Borders.TransformPoint(position, relative);
            platform.SetPosition(position);

            return platform;
        }
    }
}