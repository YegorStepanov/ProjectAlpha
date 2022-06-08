namespace Code.Services;

public sealed class Randomizer : IRandomizer
{
    public float Next(int minInclusive, int maxExclusive) =>
        UnityEngine.Random.Range(minInclusive, maxExclusive);

    public float NextProbability() =>
        UnityEngine.Random.Range(0f, 1f);
}