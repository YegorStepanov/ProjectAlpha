using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;
using VContainer;

namespace Code.Services;

public sealed class Platform : MonoBehaviour, IPlatform
{
    [SerializeField] private SpriteRenderer _platformRenderer;

    [SerializeField] private SpriteRenderer _redPointRenderer;

    private Settings _settings;
    private CancellationToken _token;

    public Borders Borders => _platformRenderer.bounds.AsBorders();

    public Borders RedPointBorders => _redPointRenderer.bounds.AsBorders();

    [Inject, UsedImplicitly]
    public void Construct(Settings settings, CancellationToken token) =>
        _settings = settings;

    private void Awake() =>
        _token = this.GetCancellationTokenOnDestroy();

    public void SetPosition(Vector2 position, Relative relative = Relative.Center)
    {
        Vector2 pos = position.Shift(Borders, relative);
        transform.position = pos;
    }

    public void SetSize(Vector2 scale)
    {
        Transform t = _platformRenderer.transform;
        Vector2 scaledAABB = _platformRenderer.bounds.size;
        Vector2 spriteSize = scaledAABB / t.localScale;
        t.localScale = scale / spriteSize;
    }

    public async UniTask MoveAsync(float destinationX) =>
        await transform.DOMoveX(destinationX, _settings.MovementSpeed)
            .SetEase(Ease.OutQuad)
            .SetSpeedBased()
            .WithCancellation(_token);

    public bool IsInsideRedPoint(float point) =>
        _redPointRenderer.color.a != 0 && point >= RedPointBorders.Left && point <= RedPointBorders.Right;

    public void ToggleRedPoint(bool enable) =>
        _redPointRenderer.color = _redPointRenderer.color with { a = enable ? 1 : 0 };

    public UniTask FadeOutRedPointAsync() =>
        _redPointRenderer.DOFade(0f, _settings.FadeOutRedPointSpeed).WithCancellation(_token);

    [Serializable]
    public class Settings
    {
        public float MovementSpeed = 10f;
        public float FadeOutRedPointSpeed = 0.3f;
    }
}