namespace Code.Services;

public interface IRandomizer
{
    int Next(int maxExclusive);
    int Next(int minInclusive, int maxExclusive);
    int NextExcept(int maxExclusive, int exclude);
    int NextExcept(int minInclusive, int maxExclusive, int exclude);
    float NextProbability();
}