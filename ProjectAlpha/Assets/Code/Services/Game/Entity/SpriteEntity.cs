using UnityEngine;

namespace Code.Services;

public abstract class SpriteEntity : Entity
{
    [SerializeField] protected SpriteRenderer _sprite;

    public override Borders Borders => _sprite.bounds.AsBorders();

    public void SetSize(Vector2 scale)
    {
        Vector2 scaledAABB = _sprite.bounds.size;
        Vector2 spriteSize = scaledAABB / _sprite.transform.localScale;
        _sprite.transform.localScale = scale / spriteSize;
    }
}