using System.Threading;
using Code.Extensions;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using VContainer;

namespace Code.Services.Entities
{
    public sealed class Hero : SlicedSpriteEntity, IHero
    {
        [SerializeField] private HeroAnimator _animator;

        private IHeroAnimations _animations;
        private Settings _settings;

        public bool IsFlipped => transform.localScale.y < 0;

        [Inject, UsedImplicitly]
        private void Construct(IHeroAnimations animations, Settings settings)
        {
            _animations = animations;
            _settings = settings;
        }

        public async UniTask MoveAsync(float destinationX, CancellationToken token)
        {
            _animator.PlayMove();
            await _animations.Move(transform, destinationX, _settings.MovementSpeed, token);
            _animator.PlayStay();
        }

        public void Squatting(CancellationToken stopToken) =>
            _animations.Squatting(transform, _settings.SquatOffset, _settings.SquatSpeed, stopToken);

        public UniTask FallAsync(float destinationY) =>
            _animations.Fall(transform, destinationY, _settings.FallingSpeed, DestroyToken);

        public UniTask KickAsync() =>
            _animator.PlayKickAsync(DestroyToken);

        public void Flip()
        {
            Vector3 scale = transform.localScale;

            scale.y *= -1;
            transform.localScale = scale;

            float sign = Mathf.Sign(scale.y);
            transform.position = transform.position.ShiftY(sign * Borders.Height);
        }

        [System.Serializable]
        public class Settings
        {
            public float MovementSpeed = 5f;
            public float FallingSpeed = 30f;
            public float SquatSpeed = 5f;
            public float SquatOffset = 0.8f;
        }
    }
}
