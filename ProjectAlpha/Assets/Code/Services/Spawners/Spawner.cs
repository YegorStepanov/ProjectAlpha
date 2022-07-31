using Code.AddressableAssets;
using Cysharp.Threading.Tasks;

namespace Code.Services.Spawners;

public abstract class Spawner<T>
{
    private readonly IAsyncPool<T> _pool;

    protected Spawner(IAsyncPool<T> pool) => _pool = pool;

    public void DespawnAll() =>
        _pool.DespawnAll();

    protected UniTask<T> SpawnAsync() =>
        _pool.SpawnAsync();

    protected void Despawn(T obj) =>
        _pool.Despawn(obj);
}
