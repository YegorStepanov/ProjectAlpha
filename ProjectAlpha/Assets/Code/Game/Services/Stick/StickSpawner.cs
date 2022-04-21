using System;
using Code.Extensions;

namespace Code.Game
{
    public sealed class StickSpawner
    {
        private readonly StickController.Pool pool;
        private readonly CameraService cameraService;
        private readonly WidthGenerator widthGenerator;
        private readonly Settings settings;

        public float StickWidth => settings.StickWidth;

        public StickSpawner(
            StickController.Pool pool,
            CameraService cameraService,
            // GameSettings gameSettings, 
            WidthGenerator widthGenerator)
        {
            this.pool = pool;
            this.cameraService = cameraService;
            this.widthGenerator = widthGenerator;

            settings = new Settings();
        }

        public IStickController Spawn(float positionX)
        {
            StickController stick = pool.Spawn();
            stick.Position = stick.Position.WithX(positionX);
            stick.Width = settings.StickWidth;
            return stick;
        }

        [Serializable]
        public class Settings
        {
            public float StickWidth = 0.04f;
        }
    }
}