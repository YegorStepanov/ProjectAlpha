using Code.VContainer;
using Cysharp.Threading.Tasks;

namespace Code.Services;

public abstract class Spawner<T>
{
    private readonly IAsyncPool<T> _pool;

    protected Spawner(IAsyncPool<T> pool) => _pool = pool;

    protected UniTask<T> SpawnAsync() => _pool.SpawnAsync();
    public void DespawnAll() => _pool.DespawnAll();
    protected void Despawn(T obj) => _pool.Despawn(obj);
}
