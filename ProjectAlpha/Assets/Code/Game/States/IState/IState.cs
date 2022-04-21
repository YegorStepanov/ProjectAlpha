using Cysharp.Threading.Tasks;

namespace Code.Game.States
{
    public interface IState : IExitState
    {
        UniTaskVoid EnterAsync();
    }
}