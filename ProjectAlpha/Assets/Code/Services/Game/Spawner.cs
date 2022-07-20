using Code.VContainer;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Services;

public abstract class Spawner<T>
{
    private readonly IAsyncPool<T> _pool;
    private T _preloaded;

    protected Spawner(IAsyncPool<T> pool) => _pool = pool;

    private async UniTask PreloadOne() =>
        _preloaded = await _pool.SpawnAsync();

    protected UniTask<T> SpawnAsync() => _pool.SpawnAsync();

    public void DespawnAll() => _pool.DespawnAll();
    protected void Despawn(T obj) => _pool.Despawn(obj);

    protected T PopUnsafe()
    {
        if (_preloaded.IsUnityNull())
            Debug.LogWarning("preloaded is default: " + GetType().Name);

        T value = _preloaded;
        _preloaded = default;
        return value;
    }
}
