using System.Diagnostics.CodeAnalysis;
using Cysharp.Threading.Tasks;

namespace Code.States;

[SuppressMessage("ReSharper", "TypeParameterCanBeVariant", Justification = "Due to performance reasons")]
public interface IArgState<TArg> : IExitState
{
    UniTaskVoid EnterAsync(TArg args);
}