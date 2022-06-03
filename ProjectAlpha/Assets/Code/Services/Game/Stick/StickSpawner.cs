using System;
using Code.VContainer;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Services;

public sealed class StickSpawner
{
    private readonly IAsyncPool<Stick> _pool;
    private readonly Settings _settings;

    public StickSpawner(IAsyncPool<Stick> pool, Settings settings)
    {
        _pool = pool;
        _settings = settings;
    }

    public async UniTask<IStick> CreateStickAsync(Vector2 position)
    {
        Stick stick = await _pool.SpawnAsync();
        stick.ResetStick();

        stick.SetPosition(position);
        stick.SetWidth(_settings.StickWidth);
        return stick;
    }
    
    public void DespawnAll() =>
        _pool.DespawnAll();

    [Serializable]
    public class Settings
    {
        public float StickWidth = 0.04f;
    }
}