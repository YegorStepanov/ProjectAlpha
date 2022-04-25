namespace Code
{
    //replace it with readonly struct that passes by IN reference anyway
    public sealed record Borders(float Top, float Bottom, float Left, float Right)
    {
        // public Vector2 Center => new((Left + Right) / 2f, (Top + Bottom) / 2f);
        public float Width => Right - Left;
        public float Height => Top - Bottom;
    }
}