using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;
using VContainer;

namespace Code.Services;

public sealed class HeroController : MonoBehaviour, IHeroController
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private HeroAnimator _animator;

    private Settings _settings;
    private CancellationToken _token;

    private IStickController _stick;

    public Borders Borders => _spriteRenderer.bounds.AsBorders();

    public float HandOffset => _settings.HandOffset; //remove it

    [Inject, UsedImplicitly]
    public void Construct(Settings settings) =>
        _settings = settings;

    private void Awake() =>
        _token = this.GetCancellationTokenOnDestroy();

    public async UniTask MoveAsync(float destinationX)
    {
        _animator.PlayMove();

        await transform.DOMoveX(destinationX, _settings.MovementSpeed)
            .SetEase(Ease.Linear)
            .SetSpeedBased()
            .WithCancellation(_token);

        _animator.PlayStay();
    }

    public async UniTask FellAsync() =>
        await transform.DOMoveY(_settings.FallingDestination, _settings.FallingSpeed)
            .SetSpeedBased()
            .SetEase(Ease.Linear)
            .WithCancellation(_token);

    public void TeleportTo(Vector2 destination, Relative relative) =>
        transform.position = Borders.GetRelativePoint(destination, relative);

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