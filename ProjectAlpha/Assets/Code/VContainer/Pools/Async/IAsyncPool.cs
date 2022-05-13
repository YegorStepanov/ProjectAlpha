using Cysharp.Threading.Tasks;

namespace Code.VContainer;

public interface IAsyncPool<T>
{
    int Capacity { get; }
    bool CanBeSpawned { get; }
    UniTask<T> SpawnAsync();
    void Despawn(T value);
}