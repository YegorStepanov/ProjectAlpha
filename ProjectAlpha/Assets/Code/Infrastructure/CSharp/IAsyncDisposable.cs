using Cysharp.Threading.Tasks;
using JetBrains.Annotations;

namespace System;

[UsedImplicitly]
public interface IAsyncDisposable
{
    public UniTask DisposeAsync();
}
