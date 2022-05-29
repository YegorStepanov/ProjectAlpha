using UnityEngine;

namespace Code;

public readonly record struct Borders(float Top, float Bottom, float Left, float Right)
{
    public Vector2 Center => new((Left + Right) / 2f, (Top + Bottom) / 2f);
    public float Width => Right - Left;
    public float Height => Top - Bottom;
}