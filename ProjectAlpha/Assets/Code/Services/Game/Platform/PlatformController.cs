using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Code.Services;

public sealed class PlatformController : MonoBehaviour, IPlatformController
{
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private Settings _settings;

    public Vector2 Position => transform.position;

    public Borders Borders => _spriteRenderer.bounds.AsBorders();

    [Inject]
    public void Construct(Settings settings) =>
        _settings = settings;

    public void SetPosition(Vector2 position)
    {
        transform.position = position;
    }

    public void SetSize(Vector2 scale)
    {
        Vector2 spriteSize = _spriteRenderer.bounds.size;
        transform.localScale = scale / spriteSize;
    }

    public async UniTask MoveAsync(float destinationX) =>
        await transform.DOMoveX(destinationX, _settings.MovementSpeed)
            .SetEase(Ease.OutQuad)
            .SetSpeedBased();

    public Vector2 GetRelativePosition(Vector2 position, Relative relative) =>
        Borders.TransformPoint(position, relative);

    [Serializable]
    public class Settings
    {
        public float MovementSpeed = 10f;
    }

    public class Pool : MonoMemoryPool<PlatformController> { }
}