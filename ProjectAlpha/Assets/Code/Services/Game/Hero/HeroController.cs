using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Code.Services;

public interface IHeroController
{
    float HandOffset { get; } //todo: OffsetToItem/Stick?
    UniTask MoveAsync(float destinationX);
    void TeleportTo(Vector2 destination, Relative relative);
    UniTask FellAsync();
}

public sealed class HeroController : MonoBehaviour, IHeroController
{
    [field: SerializeField] private SpriteRenderer spriteRenderer;
    [field: SerializeField] private Transform bottomRightPivot;

    public Borders Borders => spriteRenderer.bounds.AsBorders();

    public float HandOffset => 0.25f;

    public async UniTask MoveAsync(float destinationX) =>
        await transform.DOMoveX(destinationX, 4).SetEase(Ease.Linear).SetSpeedBased(true);

    public async UniTask FellAsync() =>
        await transform.DOMoveY(-10, -9.8f).SetSpeedBased(true);

    public void TeleportTo(Vector2 destination, Relative relative) =>
        transform.position = Borders.TransformPoint(destination, relative);
}