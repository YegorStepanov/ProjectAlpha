using System;
using System.Collections.Generic;
using Code.VContainer;
using UnityEngine;

namespace Code.Services;

public sealed class StickSpawner
{
    private readonly MonoBehaviourPool<StickController> _pool;
    private readonly Settings _settings;

    private readonly List<StickController> _sticks;

    private int _stickIndex;

    public StickSpawner(MonoBehaviourPool<StickController> pool, Settings settings)
    {
        _pool = pool;
        _settings = settings;
        _sticks = new List<StickController>(_pool.Count);
        _stickIndex = 0;
    }

    public float StickWidth => _settings.StickWidth;

    public IStickController CreateStick(Vector2 position)
    {
        if (_pool.TrySpawn(out StickController stick))
            _sticks.Add(stick);
        else
        {
            stick = _sticks[_stickIndex];
            _stickIndex = (_stickIndex + 1) % _sticks.Count;
            stick.ResetStick();
        }

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