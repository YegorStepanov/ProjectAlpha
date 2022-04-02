using System.Diagnostics.CodeAnalysis;

namespace Code.Game.States
{
    [SuppressMessage("ReSharper", "TypeParameterCanBeVariant", Justification = "Due to performance reasons")]
    public interface IStateWithParameter<TParameter> : IExitState
    {
        void Enter(TParameter payload);
    }
}