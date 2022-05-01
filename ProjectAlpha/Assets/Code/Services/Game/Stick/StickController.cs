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
    [SerializeField] private Transform stick;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private Settings settings;
    private TweenerCore<Vector3, Vector3, VectorOptions> increasingTweener;

    public float Width
    {
        get => stick.localScale.x * Borders.Width;
        set => stick.localScale = stick.localScale.WithX(value) / Borders.Width;
    }

    public Vector2 Position
    {
        get => stick.position;
        set => stick.position = value;
    }

    [Inject]
    public void Construct(Settings settings) =>
        this.settings = settings;

    private void Awake() =>
        ResetHeight();

    public Borders Borders => spriteRenderer.bounds.AsBorders();

    public void StartIncreasing() =>
        increasingTweener = stick.DOScaleY(float.MaxValue, settings.IncreaseSpeed)
            .SetSpeedBased();

    public void StopIncreasing() =>
        increasingTweener?.Kill();

    public async UniTask RotateAsync() =>
        await stick.DORotate(settings.RotationDestination, settings.RotationTime)
            .SetEase(Ease.InQuad)
            .SetDelay(settings.RotationDelay)
            .WithCancellation(this.GetCancellationTokenOnDestroy());

    private void ResetHeight() =>
        stick.localScale = stick.localScale.WithY(0f);

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