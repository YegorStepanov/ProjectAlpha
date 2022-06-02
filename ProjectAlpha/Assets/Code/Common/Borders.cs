using UnityEngine;

namespace Code;

public readonly record struct Borders(float Top, float Bottom, float Left, float Right)
{
    public float Width => Right - Left;
    public float Height => Top - Bottom;

    public float HalfWidth => Width / 2f;
    public float HalfHeight => Height / 2f;
    
    public Vector2 LeftTop => new(Left, Top);
    public Vector2 LeftCenter => new(Left, CenterY);
    public Vector2 LeftBottom => new(Left, Bottom);
    
    public Vector2 RightTop => new(Right, Top);
    public Vector2 RightCenter => new(Right, CenterY);
    public Vector2 RightBottom => new(Right, Bottom);
    
    public Vector2 CenterTop => new(CenterX, Top);
    public Vector2 Center => new(CenterX, CenterY);
    public Vector2 CenterBottom => new(CenterX, Bottom);
    
    private float CenterX => (Left + Right) / 2f;   
    private float CenterY => (Top + Bottom) / 2f;

    public bool Intersect(Borders other) =>
        !(other.Left > Right || other.Right < Left || other.Top < Bottom || other.Bottom > Top);

    public bool IsInside(Borders other) =>
        other.Left <= Left && other.Right >= Right && other.Top >= Top && other.Bottom <= Bottom;
}