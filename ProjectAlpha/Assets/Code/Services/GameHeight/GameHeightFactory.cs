using Code.Extensions;

namespace Code.Services
{
    public sealed class GameHeightFactory
    {
        private readonly ICamera _camera;
        private readonly GameSettings _settings;

        private float Height => _camera.ViewportToWorldHeight(_settings.ViewportHeight);
        private float PositionY => _camera.ViewportToWorldPosY(_settings.ViewportHeight);
        private float CameraMovementYOnGameStart => _camera.ViewportToWorldHeight(_settings.ViewportCameraMovementYOnGameStart);

        public GameHeightFactory(ICamera camera, GameSettings settings)
        {
            _camera = camera;
            _settings = settings;
        }

        public GameHeight CreateStartHeight() =>
            new(PositionY, Height + CameraMovementYOnGameStart);

        public GameHeight CreateRestartHeight() =>
            new(PositionY + CameraMovementYOnGameStart, Height + CameraMovementYOnGameStart);
    }
}