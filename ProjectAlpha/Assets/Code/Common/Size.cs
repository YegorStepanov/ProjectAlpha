namespace Code;

public readonly struct Size
{
    public int Width { get; }
    public int Height { get; }

    public Size(int width, int height) =>
        (Width, Height) = (width, height);
}