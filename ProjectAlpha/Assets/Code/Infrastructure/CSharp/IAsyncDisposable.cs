using Cysharp.Threading.Tasks;
using JetBrains.Annotations;

// ReSharper disable once CheckNamespace
namespace System
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public interface IAsyncDisposable
    {
        public UniTask DisposeAsync();
    }
}