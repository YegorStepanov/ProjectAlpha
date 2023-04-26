using Code.Services;
using Cysharp.Threading.Tasks;

namespace Code.States
{
    public interface IExitState { }

    public interface IState : IExitState
    {
        UniTaskVoid EnterAsync(IGameStateMachine stateMachine);
    }

    public interface IState<in TArg> : IExitState
    {
        UniTaskVoid EnterAsync(TArg arg, IGameStateMachine stateMachine);
    }
}
