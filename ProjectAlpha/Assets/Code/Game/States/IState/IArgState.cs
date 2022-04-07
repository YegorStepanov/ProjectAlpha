using System.Diagnostics.CodeAnalysis;

namespace Code.Game.States
{
    [SuppressMessage("ReSharper", "TypeParameterCanBeVariant", Justification = "Due to performance reasons")]
    public interface IArgState<TArg> : IExitState
    {
        void Enter(TArg payload);
    }
}