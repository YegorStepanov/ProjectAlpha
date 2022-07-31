using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Code.AddressableAssets;

//it will be the decorator when VContainer adds support for it
public sealed class RecyclablePool<T> : IAsyncPool<T>
{
    private readonly IAsyncPool<T> _pool;
    private readonly List<T> _cachedValues;
    private int _cacheIndex;

    public int Capacity { get; }
    public bool CanBeSpawned => _pool.CanBeSpawned;

    public RecyclablePool(IAsyncPool<T> pool)
    {
        _pool = pool;
        _cachedValues = new List<T>(_pool.Capacity);
        Capacity = _pool.Capacity;
    }

    public async UniTask<T> SpawnAsync() => CanBeSpawned
        ? await SpawnFromPool()
        : GetFromCache();

    public void Despawn(T value)
    {
        TryRemoveFromCache(value);
        _pool.Despawn(value);
    }

    public void DespawnAll()
    {
        foreach (T value in _cachedValues)
            _pool.Despawn(value);

        _cacheIndex = 0;
        _cachedValues.Clear();
    }

    private async UniTask<T> SpawnFromPool()
    {
        T stick = await _pool.SpawnAsync();
        _cachedValues.Add(stick);
        return stick;
    }

    private T GetFromCache()
    {
        T stick = _cachedValues[_cacheIndex];
        _cacheIndex = (_cacheIndex + 1) % _cachedValues.Count;
        return stick;
    }

    private void TryRemoveFromCache(T value)
    {
        int index = _cachedValues.IndexOf(value);
        if (index == -1) return;
        _cachedValues.RemoveAt(index);

        if (_cacheIndex > 0 && index > _cacheIndex)
            _cacheIndex--;
    }
}
