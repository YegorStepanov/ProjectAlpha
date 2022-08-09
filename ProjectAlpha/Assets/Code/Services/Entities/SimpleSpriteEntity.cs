using Code.Common;
using Code.Extensions;
using UnityEngine;

namespace Code.Services.Entities;

public abstract class SimpleSpriteEntity : Entity
{
    [SerializeField] protected SpriteRenderer _sprite;

    public override Borders Borders => _sprite.bounds.AsBorders();

    private void OnValidate() =>
        Debug.Assert(_sprite.drawMode == SpriteDrawMode.Simple, $"{name}: Sprite is sliced or tiled, use {nameof(SlicedSpriteEntity)} instead");

    public void SetSize(Vector2 worldSize)
    {
        Vector2 scaledAABB = _sprite.bounds.size;
        Vector2 spriteSize = scaledAABB / _sprite.transform.localScale;
        _sprite.transform.localScale = worldSize / spriteSize;
    }
}
