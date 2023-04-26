using UnityEngine;
using VContainer.Unity;

namespace Code.Services
{
    public sealed class CameraRestorer : IStartable, ICameraRestorer
    {
        private readonly ICamera _camera;
        private Vector2 _initialPosition;

        public CameraRestorer(ICamera camera) =>
            _camera = camera;

        void IStartable.Start() =>
            _initialPosition = _camera.Borders.Center;

        public void RestorePosition() =>
            _camera.SetPosition(_initialPosition);

        public void RestorePositionX() =>
            _camera.SetPosition(new Vector2(_initialPosition.x, _camera.Borders.Center.y));

        public void RestorePositionY() =>
            _camera.SetPosition(new Vector2(_camera.Borders.Center.x, _initialPosition.y));
    }
}
