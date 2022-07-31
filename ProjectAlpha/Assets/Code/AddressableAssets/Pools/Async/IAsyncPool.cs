using Cysharp.Threading.Tasks;

namespace Code.AddressableAssets;

public interface IAsyncPool<T>
{
    int Capacity { get; }
    bool CanBeSpawned { get; }
    UniTask<T> SpawnAsync();
    void Despawn(T value);
    //todo: void WarmUp(int count);
    void DespawnAll();
}
