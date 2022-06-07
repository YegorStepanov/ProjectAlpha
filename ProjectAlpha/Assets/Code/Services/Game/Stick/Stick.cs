using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using JetBrains.Annotations;
using UnityEngine;
using VContainer;

namespace Code.Services;

//Bug: stick speed depends on PPU of sprite `rectangle`  
public sealed class Stick : MonoBehaviour, IStick
{
    [SerializeField] private Transform _stick;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Transform _arrow;

    private Settings _settings;
    private CancellationToken _token;
    private TweenerCore<Vector3, Vector3, VectorOptions> _increaseTweener;

    public Vector2 ArrowPosition => _arrow.position;

    public Borders Borders => _spriteRenderer.bounds.AsBorders();

    [Inject, UsedImplicitly]
    public void Construct(Settings settings) =>
        _settings = settings;

    private void Awake()
    {
        _token = this.GetCancellationTokenOnDestroy();
        ResetStick();
    }

    public void SetPosition(Vector2 position) =>
        _stick.position = position;

    public void SetWidth(float width) =>
        _stick.localScale = _stick.localScale.WithX(width / Borders.Width);
    
    public void ResetStick()
    {
        _stick.rotation = Quaternion.identity;
        _stick.localScale = _stick.localScale.WithY(0f);
    }

    public void StartIncreasing() =>
        _increaseTweener = _stick.DOScaleY(float.MaxValue, _settings.IncreaseSpeed)
            .SetSpeedBased();

    public void StopIncreasing() =>
        _increaseTweener?.Kill();

    public async UniTask RotateAsync() =>
        await _stick.DORotate(_settings.RotationDestination, _settings.RotationTime)
            .SetEase(Ease.InQuad)
            .SetDelay(_settings.RotationDelay)
            .WithCancellation(_token);

    [System.Serializable]
    public class Settings
    {
        public float IncreaseSpeed = 25f;
        public Vector3 RotationDestination = new(0, 0, -90);
        public float RotationTime = 0.4f;
        public float RotationDelay = 0.3f;
    }
}