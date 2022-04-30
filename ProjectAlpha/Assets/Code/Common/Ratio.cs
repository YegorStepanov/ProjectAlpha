namespace Code;

public readonly record struct Ratio(float Min, float Max)
{
    public static Ratio operator *(Ratio ratio, float value) =>
        new(ratio.Min * value, ratio.Max * value);
}