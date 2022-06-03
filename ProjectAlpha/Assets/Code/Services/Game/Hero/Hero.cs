using System;
using System.Threading;
using Code.HeroAnimators;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;
using VContainer;

namespace Code.Services;

public sealed class Hero : MonoBehaviour, IHero
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private HeroAnimator _animator;

    private Settings _settings;
    private CancellationToken _token;

    private IStick _stick;

    public Borders Borders => _spriteRenderer.bounds.AsBorders();

    public float HandOffset => _settings.HandOffset; //remove it

    public bool IsFlipped => transform.localScale.y < 0;

    [Inject, UsedImplicitly]
    public void Construct(Settings settings) =>
        _settings = settings;

    //cache transform in Construct
    private void Awake() =>
        _token = this.GetCancellationTokenOnDestroy();

    public void SetPosition(Vector2 destination, Relative relative) =>
        transform.position = destination.Shift(Borders, relative);

    // ReSharper disable Unity.InefficientPropertyAccess
    public void Flip() =>
        transform.localScale = transform.localScale with { y = transform.localScale.y * -1 };

    public async UniTask MoveAsync(float destinationX, CancellationToken token = default)
    {
        if (token == default)
            token = _token;

        _animator.PlayMove();

        await transform.DOMoveX(destinationX, _settings.MovementSpeed)
            .SetEase(Ease.Linear)
            .SetSpeedBased()
            .WithCancellation(token);

        _animator.PlayStay();
    }

    public async UniTask FellAsync() =>
        await transform.DOMoveY(_settings.FallingDestination, _settings.FallingSpeed)
            .SetSpeedBased()
            .SetEase(Ease.Linear)
            .WithCancellation(_token);

    public UniTask KickAsync() =>
        _animator.PlayKickAsync();

    [Serializable]
    public class Settings
    {
        public float HandOffset = 0.25f;
        public float MovementSpeed = 5f;
        public float FallingSpeed = 30f;
        public float FallingDestination = -10f;
    }
}