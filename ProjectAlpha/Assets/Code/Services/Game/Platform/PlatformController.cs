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
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private Settings _settings;
    private CancellationToken _token;

    public Vector2 Position => transform.position;

    public Borders Borders => _spriteRenderer.bounds.AsBorders();

    [Inject, UsedImplicitly]
    public void Construct(Settings settings) =>
        _settings = settings;

    private void Awake() =>
        _token = this.GetCancellationTokenOnDestroy();

    public void SetPosition(Vector2 position)
    {
        transform.position = position;
    }

    public void SetSize(Vector2 scale)
    {
        Transform t = transform;
        Vector2 scaledAABB = _spriteRenderer.bounds.size;
        Vector2 spriteSize = scaledAABB / t.localScale;
        t.localScale = scale / spriteSize;
    }

    public async UniTask MoveAsync(float destinationX) =>
        await transform.DOMoveX(destinationX, _settings.MovementSpeed)
            .SetEase(Ease.OutQuad)
            .SetSpeedBased()
            .WithCancellation(_token);

    public Vector2 GetRelativePosition(Vector2 position, Relative relative) =>
        Borders.TransformPoint(position, relative);

    [Serializable]
    public class Settings
    {
        public float MovementSpeed = 10f;
    }
}