using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.VContainer;

public abstract class AsyncPool<TValue> : IAsyncPool<TValue>
{
    private readonly int _initialSize;

    private TValue[] _pool;
    private int _activeCount;

    public int Capacity { get; }

    protected AsyncPool(int initialSize, int capacity)
    {
        _initialSize = initialSize;
        Capacity = capacity;
    }

    public async UniTask<(TValue, bool)> SpawnAsync()
    {
        _pool ??= await CreatePoolAsync();
        
        if (!IsIndexCorrectForSpawn()) 
            return (default, false);
        
        TValue value = await Pop();
        OnSpawned(value);

        return (value, true);
    }

    public void Despawn(TValue value)
    {
        if (_pool is null)
        {
            Debug.LogWarning("Pool is not initialized");
            return;
        }
        
        if (!IsIndexCorrectForDespawn())
        {
            Debug.LogWarning("Incorrect Index: " + _activeCount);
            return;
        }
        
        Push(value);
        OnDespawned(value);
    }

    protected abstract UniTask<TValue> CreateAsync();

    protected abstract void OnSpawned(TValue instance);

    protected abstract void OnDespawned(TValue instance);

    private async UniTask<TValue[]> CreatePoolAsync()
    {
        var pool = new TValue[Capacity];
        
        if(_initialSize == 0)
            return pool;
        
        for (int i = 0; i < _initialSize; i++)
        {
            pool[i] = await CreateAsync();
            OnDespawned(pool[i]);
        }

        OnSpawned(pool[0]);

        return pool;
    }

    private async UniTask<TValue> Pop()
    {
        if (IsDefault(_pool[_activeCount]))
            _pool[_activeCount] = await CreateAsync();

        TValue value = _pool[_activeCount];
        _activeCount++;
        return value;
    }
    
    private void Push(TValue value)
    {
        _activeCount--;
        _pool[_activeCount] = value;
    }

    private bool IsIndexCorrectForSpawn() =>
        _activeCount >= 0 && _activeCount < _pool.Length;

    private bool IsIndexCorrectForDespawn() =>
        _activeCount > 0 && _activeCount <= _pool.Length;

    private static bool IsDefault(TValue value) =>
        EqualityComparer<TValue>.Default.Equals(value, default);
}