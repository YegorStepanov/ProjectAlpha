using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using Zenject;

namespace Code.Services;

public sealed class StickController : MonoBehaviour, IStickController
{
    [SerializeField] private Transform _stick;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private Settings _settings;
    private TweenerCore<Vector3, Vector3, VectorOptions> _increaseTweener;

    public float Width
    {
        get => _stick.localScale.x * Borders.Width;
        set => _stick.localScale = _stick.localScale.WithX(value) / Borders.Width;
    }

    public Vector2 Position
    {
        get => _stick.position;
        set => _stick.position = value;
    }

    [Inject]
    public void Construct(Settings settings) =>
        _settings = settings;

    private void Awake() =>
        ResetHeight();

    public Borders Borders => _spriteRenderer.bounds.AsBorders();

    public void StartIncreasing() =>
        _increaseTweener = _stick.DOScaleY(float.MaxValue, _settings.IncreaseSpeed)
            .SetSpeedBased();

    public void StopIncreasing() =>
        _increaseTweener?.Kill();

    public async UniTask RotateAsync() =>
        await _stick.DORotate(_settings.RotationDestination, _settings.RotationTime)
            .SetEase(Ease.InQuad)
            .SetDelay(_settings.RotationDelay)
            .WithCancellation(this.GetCancellationTokenOnDestroy());

    private void ResetHeight() =>
        _stick.localScale = _stick.localScale.WithY(0f);

    [Serializable]
    public class Settings
    {
        public float IncreaseSpeed = 25f;
        public Vector3 RotationDestination = new(0, 0, -90);
        public float RotationTime = 0.4f;
        public float RotationDelay = 0.3f;
    }

    public class Pool : MonoMemoryPool<StickController> { }
}