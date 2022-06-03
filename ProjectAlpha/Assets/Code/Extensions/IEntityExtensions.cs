using Code.Services;
using UnityEngine;

namespace Code;

// ReSharper disable once InconsistentNaming
public static class IEntityExtensions
{
    public static bool Intersect(this IEntity entity, IEntity other) =>
        entity.Borders.Intersect(other.Borders);

    public static bool Inside(this IEntity entity, IEntity other) =>
        entity.Borders.Inside(other.Borders);

    public static bool Contains(this IEntity entity, Vector2 point) =>
        entity.Borders.Inside(point);
}