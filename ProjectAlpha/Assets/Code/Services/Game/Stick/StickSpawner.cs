using System;

namespace Code.Services;

public sealed class StickSpawner
{
    private readonly StickController.Pool pool;
    private readonly Settings settings;

    public StickSpawner(StickController.Pool pool)
    {
        this.pool = pool;
        settings = new Settings();
    }

    public float StickWidth => settings.StickWidth;

    public IStickController Spawn(float positionX)
    {
        StickController stick = pool.Spawn();
        stick.Position = stick.Position.WithX(positionX);
        stick.Width = settings.StickWidth;
        return stick;
    }

    [Serializable]
    public class Settings
    {
        public float StickWidth = 0.04f;
    }
}