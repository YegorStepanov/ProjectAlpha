using System;
using System.Threading;
using Code.HeroAnimators;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using VContainer;

namespace Code.Services;

public sealed class Hero : SpriteEntity, IHero
{
    [SerializeField] private HeroAnimator _animator;

    private IHeroAnimations _animations;
    private Settings _settings;

    public bool IsFlipped => transform.localScale.y < 0;

    [Inject, UsedImplicitly]
    public void Construct(IHeroAnimations animations, Settings settings)
    {
        _animations = animations;
        _settings = settings;
    }

    public void ResetState()
    {
        if(IsFlipped)
            Flip();
    }

    public async UniTask MoveAsync(float destinationX, CancellationToken token)
    {
        _animator.PlayMove();
        await _animations.Move(transform, destinationX - _settings.HandOffset, _settings.MovementSpeed, token);
        _animator.PlayStay();
    }

    public void Squatting(CancellationToken token) =>
        _animations.Squatting(transform, _settings.SquatOffset, _settings.SquatSpeed, token);

    public async UniTask FallAsync(float destinationY)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(_settings.FallingDelay));
        await _animations.Fall(transform, destinationY, _settings.FallingSpeed, DestroyToken);
    }

    public UniTask KickAsync() =>
        _animator.PlayKickAsync(DestroyToken);

    public void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.y *= -1;
        transform.localScale = scale;
    }

    [System.Serializable]
    public class Settings
    {
        public float HandOffset = 0.25f;
        public float MovementSpeed = 5f;
        public float FallingSpeed = 30f;
        public float FallingDelay = 0.1f;
        public float SquatSpeed = 5f;
        public float SquatOffset = 0.8f;
    }
}
