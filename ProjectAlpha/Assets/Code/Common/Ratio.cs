namespace Code
{
    public readonly struct Ratio
    {
        public readonly float Min;
        public readonly float Max;

        public Ratio(float min, float max) =>
            (Min, Max) = (min, max);

        public static Ratio operator *(Ratio ratio, float value) =>
            new Ratio(ratio.Min * value, ratio.Max * value);
    }
}