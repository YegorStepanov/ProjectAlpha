using Code.Common;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace Code.Services.Entities
{
    public sealed class Platform : SlicedSpriteEntity, IPlatform
    {
        [SerializeField] private PlatformRedPoint _platformRedPoint;
        private IPlatformAnimations _animations;
        private Settings _settings;

        public IPlatformRedPoint PlatformRedPoint => _platformRedPoint;

        [Inject]
        private void Construct(IPlatformAnimations animations, Settings settings)
        {
            _animations = animations;
            _settings = settings;
        }

        public UniTask MoveAsync(float destinationX) =>
            _animations.Move(transform, destinationX, _settings.MovementSpeed, DestroyToken);

        public void SetSize(Vector2 worldSize)
        {
            SetSpriteSize(worldSize);
            _platformRedPoint.SetPosition(Borders.CenterTop, Relative.Top);
        }

        [System.Serializable]
        public class Settings
        {
            public float MovementSpeed = 10f;
            public float FadeOutRedPointSpeed = 0.3f;
        }
    }
}