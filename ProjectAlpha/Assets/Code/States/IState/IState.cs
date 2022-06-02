using Code.Services;
using Cysharp.Threading.Tasks;

namespace Code.States;

public interface IState : IExitState
{
    UniTaskVoid EnterAsync(IStateMachine stateMachine);
}

public interface IState<in TArg> : IExitState
{
    UniTaskVoid EnterAsync(TArg arg, IStateMachine stateMachine);
}