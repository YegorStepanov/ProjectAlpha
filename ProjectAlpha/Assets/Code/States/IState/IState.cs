using Code.Services;
using Cysharp.Threading.Tasks;

namespace Code.States;

public interface IState : IExitState
{
    UniTaskVoid EnterAsync(IStateMachine stateMachine);
}