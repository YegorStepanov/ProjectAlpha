public interface IAnimationStateReader
{
    HeroAnimatorState State { get; }
    void EnteredState(int stateHash);
    void ExitedState(int stateHash);
}