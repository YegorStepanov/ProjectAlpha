namespace Code.Services;

public interface IEntity
{
    Borders Borders { get; }
}

// ReSharper disable once InconsistentNaming
public static class IEntityExtensions
{
    public static bool Intersect(this IEntity entity, IEntity other)
    {
        Borders e = entity.Borders;
        Borders otherB = other.Borders;
        return !(otherB.Left > e.Right || otherB.Right < e.Left ||
                 otherB.Top < e.Bottom || otherB.Bottom > e.Top);
    }

    public static bool IsInside(this IEntity entity, IEntity other)
    {
        Borders e = entity.Borders;
        Borders otherB = other.Borders;
        return otherB.Left <= e.Left && otherB.Right >= e.Right &&
               otherB.Top >= e.Top && otherB.Bottom <= e.Bottom;
    }
}