using Code.States;

namespace Code.Services
{
    public interface IGameStateMachine
    {
        void Enter<TState>() where TState : class, IState;
        void Enter<TState, TArg>(TArg argument) where TState : class, IState<TArg>;
    }
}
