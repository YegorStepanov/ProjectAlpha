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

    private Settings _settings;
    private CancellationToken _token;

    public Borders Borders => _spriteRenderer.bounds.AsBorders();

    public float HandOffset => _settings.HandOffset; //remove it

    [Inject, UsedImplicitly]
    public void Construct(Settings settings) =>
        _settings = settings;

    private void Awake() =>
        _token = this.GetCancellationTokenOnDestroy();

    public async UniTask MoveAsync(float destinationX) =>
        await transform.DOMoveX(destinationX, _settings.MovementSpeed)
            .SetEase(Ease.Linear)
            .SetSpeedBased()
            .WithCancellation(_token);

    public async UniTask FellAsync() =>
        await transform.DOMoveY(_settings.FallingDestination, -_settings.FallingSpeed)
            .SetSpeedBased()
            .WithCancellation(_token);

    public void TeleportTo(Vector2 destination, Relative relative) =>
        transform.position = Borders.TransformPoint(destination, relative);

    [Serializable]
    public class Settings
    {
        public float HandOffset = 0.25f;
        public float MovementSpeed = 4f;
        public float FallingSpeed = 9.8f;
        public float FallingDestination = -10f;
    }
}