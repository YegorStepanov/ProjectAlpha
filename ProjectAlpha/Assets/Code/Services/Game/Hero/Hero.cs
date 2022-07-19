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

    public float HandOffset => _settings.HandOffset; //remove it
    public bool IsFlipped => transform.localScale.y < 0;

    [Inject, UsedImplicitly]
    public void Construct(IHeroAnimations animations, Settings settings)
    {
        _animations = animations;
        _settings = settings;
    }

    public UniTask MoveAsync(float destinationX) =>
        MoveAsync(destinationX, DestroyToken);

    public async UniTask MoveAsync(float destinationX, CancellationToken token)
    {
        _animator.PlayMove();
        await _animations.Move(transform, destinationX, _settings.MovementSpeed, token);
        _animator.PlayStay();
    }

    public UniTask SquatAsync(CancellationToken token) =>
        _animations.Squat(transform, _settings.SquatOffset, _settings.SquatSpeed, token);

    public UniTask FallAsync() =>
        _animations.Fall(transform, _settings.FallingDestination, _settings.FallingSpeed, DestroyToken);

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
        public float FallingDestination = -10f;
        public float SquatSpeed = 5f;
        public float SquatOffset = 0.8f;
    }
}
