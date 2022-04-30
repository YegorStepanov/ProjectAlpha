using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Code.Services;

public interface IPlatformController
{
    Vector2 Position { get; }

    Borders Borders { get; }
    void SetPosition(Vector2 position);
    void SetSize(Vector2 scale);
    UniTask MoveAsync(float destinationX);
    Vector2 GetRelativePosition(Vector2 position, Relative relative);
}

public sealed class PlatformController : MonoBehaviour, IPlatformController
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    public Vector2 Position => transform.position;

    public Borders Borders => spriteRenderer.bounds.AsBorders();

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
        await transform.DOMoveX(destinationX, 10)
            .SetEase(Ease.OutQuad)
            .SetSpeedBased(true);

    public Vector2 GetRelativePosition(Vector2 position, Relative relative) =>
        Borders.TransformPoint(position, relative);

    public class Pool : MonoMemoryPool<PlatformController> { }
}