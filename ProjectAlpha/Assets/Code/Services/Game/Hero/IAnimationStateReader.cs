namespace Code.HeroAnimators;

public interface IAnimationStateReader
{
    void EnteredState(int stateHash, int layerIndex);
    void ExitedState(int stateHash, int layerIndex);
}