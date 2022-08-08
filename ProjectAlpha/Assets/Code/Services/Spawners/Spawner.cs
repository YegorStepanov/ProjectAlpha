using System.Collections.Generic;
using Code.AddressableAssets;
using Cysharp.Threading.Tasks;

namespace Code.Services.Spawners;

public abstract class Spawner<T>
{
    private readonly IAsyncPool<T> _pool;
    private readonly List<T> _activeItems;

    public IReadOnlyList<T> ActiveItems => _activeItems;

    protected Spawner(IAsyncPool<T> pool)
    {
        _pool = pool;
        _activeItems = new List<T>(pool.Capacity);
    }

    public void DespawnAll()
    {
        _activeItems.Clear();
        _pool.DespawnAll();
    }

    protected async UniTask<T> SpawnAsync()
    {
        T item = await _pool.SpawnAsync();

        if (_activeItems.Count == _pool.Capacity)
            _activeItems.RemoveAt(0);

        _activeItems.Add(item);
        return item;
    }

    protected void Despawn(T item)
    {
        _activeItems.Remove(item);
        _pool.Despawn(item);
    }
}
