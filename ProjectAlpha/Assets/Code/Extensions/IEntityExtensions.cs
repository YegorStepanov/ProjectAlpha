using Code.Services.Entities;
using UnityEngine;

namespace Code.Extensions;

// ReSharper disable once InconsistentNaming
public static class IEntityExtensions
{
    public static bool Intersect<T1, T2>(this T1 entity, T2 other) where T1 : IEntity where T2 : IEntity =>
        entity.Borders.Intersect(other.Borders);

    public static bool Inside<T1, T2>(this T1 entity, T2 other) where T1 : IEntity where T2 : IEntity =>
        entity.Borders.Inside(other.Borders);

    public static bool Contains<T1>(this T1 entity, Vector2 point) where T1 : IEntity =>
        entity.Borders.Inside(point);
}
