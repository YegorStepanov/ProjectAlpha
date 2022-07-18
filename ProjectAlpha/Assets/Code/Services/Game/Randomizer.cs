using UnityEngine;

namespace Code.Services;

public sealed class Randomizer : IRandomizer
{
    public int Next(int maxExclusive) =>
        Next(0, maxExclusive);

    public int Next(int minInclusive, int maxExclusive) =>
        Random.Range(minInclusive, maxExclusive);

    public int NextExcept(int maxExclusive, int exclude) =>
        NextExcept(0, maxExclusive, exclude);

    public int NextExcept(int minInclusive, int maxExclusive, int exclude)
    {
        if (exclude < minInclusive || exclude >= maxExclusive)
            Debug.LogError(
                $"Exclude is out of range: exclude={exclude} minInclusive={minInclusive} maxExclusive={maxExclusive}");

        int value = Random.Range(minInclusive, maxExclusive - 1);

        if (value >= exclude)
            value++;

        return value;
    }

    public float NextProbability() =>
        Random.Range(0f, 1f);
}
