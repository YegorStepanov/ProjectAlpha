namespace Code;

//replace it with readonly struct that passes by IN reference anyway
//   public sealed record Borders(float Top, float Bottom, float Left, float Right)
public sealed class Borders
{
    public Borders(float top, float bottom, float left, float right)
    {
        Top = top;
        Bottom = bottom;
        Left = left;
        Right = right;
    }

    public float Top { get; }
    public float Bottom { get; }
    public float Left { get; }
    public float Right { get; }

    // public Vector2 Center => new((Left + Right) / 2f, (Top + Bottom) / 2f);
    public float Width => Right - Left;
    public float Height => Top - Bottom;
}