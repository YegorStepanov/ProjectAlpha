using System;

namespace Code.Services;

public sealed class StickSpawner
{
    private readonly StickController.Pool _pool;
    private readonly Settings _settings;

    public StickSpawner(StickController.Pool pool, Settings settings)
    {
        _pool = pool;
        _settings = settings;
    }

    public float StickWidth => _settings.StickWidth;

    public IStickController Spawn(float positionX)
    {
        StickController stick = _pool.Spawn();
        stick.Position = stick.Position.WithX(positionX);
        stick.Width = _settings.StickWidth;
        return stick;
    }

    [Serializable]
    public class Settings
    {
        public float StickWidth = 0.04f;
    }
}