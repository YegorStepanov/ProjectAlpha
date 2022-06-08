namespace Code.Services;

public interface IRandomizer
{
    float Next(int minInclusive, int maxExclusive);
    float NextProbability();
}