using System;
using Code.VContainer;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Services;

public sealed class StickSpawner
{
    private readonly IAsyncRecyclablePool<StickController> _pool;
    private readonly Settings _settings;

    public StickSpawner(IAsyncRecyclablePool<StickController> pool, Settings settings)
    {
        _pool = pool;
        _settings = settings;
    }

    public float StickWidth => _settings.StickWidth;

    public async UniTask<IStickController> CreateStickAsync(Vector2 position)
    {
        StickController stick = await _pool.SpawnAsync();
        stick.ResetStick();

        stick.Position = position;
        stick.Width = _settings.StickWidth;
        return stick;
    }

    [Serializable]
    public class Settings
    {
        public float StickWidth = 0.04f;
    }
}