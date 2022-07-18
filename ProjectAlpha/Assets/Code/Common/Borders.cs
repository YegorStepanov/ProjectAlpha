﻿using System.Text;
using UnityEngine;

namespace Code;

public readonly record struct Borders(float Top, float Bot, float Left, float Right)
{
    public static Borders Infinity =>
        new(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);

    public float Width => Right - Left;
    public float Height => Top - Bot;

    public float HalfWidth => Width / 2f;
    public float HalfHeight => Height / 2f;

    public Vector2 LeftTop => new(Left, Top);
    public Vector2 LeftCenter => new(Left, CenterY);
    public Vector2 LeftBot => new(Left, Bot);

    public Vector2 RightTop => new(Right, Top);
    public Vector2 RightCenter => new(Right, CenterY);
    public Vector2 RightBot => new(Right, Bot);

    public Vector2 CenterTop => new(CenterX, Top);
    public Vector2 Center => new(CenterX, CenterY);
    public Vector2 CenterBot => new(CenterX, Bot);

    private float CenterX => (Left + Right) / 2f;
    private float CenterY => (Top + Bot) / 2f;

    private bool PrintMembers(StringBuilder builder)
    {
        builder.Append(LeftTop);
        builder.Append(RightBot);
        return true;
    }
}
