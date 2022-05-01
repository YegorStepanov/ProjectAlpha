using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Code.Services;

public sealed class PlatformController : MonoBehaviour, IPlatformController
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    private Settings settings;

    public Vector2 Position => transform.position;

    public Borders Borders => spriteRenderer.bounds.AsBorders();

    [Inject]
    public void Construct(Settings settings) =>
        this.settings = settings;

    public void SetPosition(Vector2 position)
    {
        transform.position = position;
    }

    public void SetSize(Vector2 scale)
    {
        Vector2 spriteSize = spriteRenderer.bounds.size;
        transform.localScale = scale / spriteSize;
    }

    public async UniTask MoveAsync(float destinationX) =>
        await transform.DOMoveX(destinationX, settings.MovementSpeed)
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