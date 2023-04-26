using Code.Common;
using Code.Extensions;
using UnityEngine;

namespace Code.Services.Entities
{
    public abstract class SlicedSpriteEntity : Entity
    {
        [SerializeField] private SpriteRenderer _sprite;

        public override Borders Borders => _sprite.bounds.AsBorders();
        protected SpriteRenderer SpriteRenderer => _sprite;

        protected virtual void OnValidate()
        {
            Debug.Assert(_sprite != null, $"_sprite == null for {name} of type {GetType()}");
            Debug.Assert(_sprite.drawMode != SpriteDrawMode.Simple, $"{name}: Sprite is simple, use {nameof(SimpleSpriteEntity)} instead");
        }

        protected void SetSpriteSize(Vector2 worldSize) =>
            _sprite.size = worldSize;

        protected void SetSpriteAlpha(float alpha) =>
            _sprite.color = _sprite.color with { a = alpha };
    }
}
