using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Code.VContainer;

//it will be the decorator when VContainer adds support for it
public sealed class RecyclablePool<T> : IAsyncRecyclablePool<T>
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

    public async UniTask<T> SpawnAsync()
    {
        if (_pool.CanBeSpawned)
        {
            T stick = await _pool.SpawnAsync();
            _cachedValues.Add(stick);
            return stick;
        }
        else
        {
            T stick = _cachedValues[_cacheIndex];
            _cacheIndex = (_cacheIndex + 1) % _cachedValues.Count;
            return stick;
        }
    }

    public void Despawn(T value) =>
        _pool.Despawn(value);
}