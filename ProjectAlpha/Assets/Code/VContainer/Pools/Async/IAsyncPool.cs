using Cysharp.Threading.Tasks;

namespace Code.VContainer;

public interface IAsyncPool<TValue>
{
    int Capacity { get; }
    UniTask<(TValue, bool)> SpawnAsync();
    void Despawn(TValue value);
}