using UnityEngine;
using VContainer.Unity;

namespace Code.Services
{
    public sealed class CameraResetter : IStartable, ICameraResetter
    {
        private readonly ICamera _camera;
        private Vector2 _initialPosition;

        public CameraResetter(ICamera camera) =>
            _camera = camera;

        void IStartable.Start() =>
            _initialPosition = _camera.Borders.Center;

        public void ResetPosition() =>
            _camera.SetPosition(_initialPosition);

        public void ResetPositionX() =>
            _camera.SetPosition(new Vector2(_initialPosition.x, _camera.Borders.Center.y));

        public void ResetPositionY() =>
            _camera.SetPosition(new Vector2(_camera.Borders.Center.x, _initialPosition.y));
    }
}
