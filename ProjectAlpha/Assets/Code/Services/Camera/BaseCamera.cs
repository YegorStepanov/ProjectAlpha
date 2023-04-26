using Code.Common;
using Code.Extensions;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;
using VContainer;

namespace Code.Services
{
    [RequireComponent(typeof(Camera))]
    public sealed class BaseCamera : MonoBehaviour, ICamera
    {
        private Settings _settings;

        private Camera _camera;
        private Vector2 _size;

        public Borders Borders { get; private set; }

        [Inject, UsedImplicitly]
        private void Construct(Settings settings) =>
            _settings = settings;

        private void Awake()
        {
            _camera = GetComponent<Camera>();
            _size = GetWorldSize();
            UpdateBorders();
        }

        public void SetPosition(Vector2 position)
        {
            transform.position = position.WithZ(transform.position.z);
            UpdateBorders();
        }

        public UniTask PunchAsync() => transform
            .DOPunchPosition(
                _settings.PunchingStrength,
                _settings.PunchingDuration,
                _settings.PunchingVibrato,
                _settings.PunchingElasticity)
            .ToUniTask();

        public UniTask MoveAsync(Vector2 destination) => transform
            .DOMove(destination.WithZ(transform.position.z), 7f)
            .SetSpeedBased()
            .OnUpdate(UpdateBorders)
            .ToUniTask();

        private void UpdateBorders()
        {
            Vector3 position = transform.position;
            float halfHeight = _size.y / 2f;
            float halfWidth = _size.x / 2f;

            Borders = new Borders(
                Top: position.y + halfHeight,
                Bot: position.y - halfHeight,
                Left: position.x - halfWidth,
                Right: position.x + halfWidth);
        }

        private Vector2 GetWorldSize()
        {
            float height = _camera.orthographicSize * 2;
            float width = height * _camera.aspect;
            return new Vector2(width, height);
        }

        [System.Serializable]
        public class Settings
        {
            public Vector3 PunchingStrength = Vector3.up;
            public float PunchingDuration = 0.3f;
            public int PunchingVibrato = 10;
            public float PunchingElasticity = 1f;
        }
    }
}
