using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;
using VContainer;

namespace Code.Services;

public sealed class PlatformController : MonoBehaviour, IPlatformController
{
    [SerializeField] private SpriteRenderer _platformRenderer;

    [SerializeField] private SpriteRenderer _redPointRenderer;

    private Settings _settings;
    private CancellationToken _token;

    public Vector2 Position => transform.position;

    public Borders Borders => _platformRenderer.bounds.AsBorders();

    private Borders RedPointBorders => _redPointRenderer.bounds.AsBorders();

    [Inject, UsedImplicitly]
    public void Construct(Settings settings) =>
        _settings = settings;

    private void Awake() =>
        _token = this.GetCancellationTokenOnDestroy();

    public void SetPosition(Vector2 position, Relative relative = Relative.Center)
    {
        Vector2 pos = Borders.GetRelativePoint(position, relative);
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

    public Vector2 GetRelativePosition(Vector2 position, Relative relative) =>
        Borders.GetRelativePoint(position, relative);

    public bool IsInsideRedPoint(float point) =>
        point >= RedPointBorders.Left && point <= RedPointBorders.Right;
    
    [Serializable]
    public class Settings
    {
        public float MovementSpeed = 10f;
    }
}