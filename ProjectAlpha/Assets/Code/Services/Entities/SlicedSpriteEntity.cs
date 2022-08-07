using Code.Common;
using Code.Extensions;
using UnityEngine;

namespace Code.Services.Entities;

public abstract class SlicedSpriteEntity : Entity
{
    [SerializeField] protected SpriteRenderer _sprite;

    public override Borders Borders => _sprite.bounds.AsBorders();

    private void OnValidate() =>
        Debug.Assert(_sprite.drawMode != SpriteDrawMode.Simple, $"{name}: Sprite is simple, use {nameof(SimpleSpriteEntity)} instead");

    public virtual void SetSize(Vector2 worldSize) =>
        _sprite.size = worldSize;
}
