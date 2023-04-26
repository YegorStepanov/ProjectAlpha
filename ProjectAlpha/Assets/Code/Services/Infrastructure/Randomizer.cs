using UnityEngine;

namespace Code.Services.Infrastructure
{
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
            int value = Random.Range(minInclusive, maxExclusive - 1);

            if (exclude >= 0 && value >= exclude)
                value++;

            return value;
        }

        public float NextProbability() =>
            Random.Range(0f, 1f);
    }
}
