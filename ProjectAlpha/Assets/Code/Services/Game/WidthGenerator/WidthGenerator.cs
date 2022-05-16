using Code.VContainer;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Services;

public abstract class WidthGenerator : ScriptableObject
{
    public abstract void Reset();
    public abstract float NextWidth();
}

public sealed class WidthGeneratorSpawner
{
    private readonly IAsyncRecyclablePool<WidthGenerator> _pool;

    public WidthGeneratorSpawner(IAsyncRecyclablePool<WidthGenerator> pool) =>
        _pool = pool;

    public async UniTask<WidthGenerator> CreateAsync()
    {
        WidthGenerator stick = await _pool.SpawnAsync();
        stick.Reset();
        return stick;
    }
}