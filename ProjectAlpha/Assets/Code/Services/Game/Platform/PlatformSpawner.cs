using UnityEngine;

namespace Code.Services;

public sealed class PlatformSpawner
{
    private readonly CameraController cameraController;
    private readonly PlatformController.Pool pool;

    public PlatformSpawner(PlatformController.Pool pool, CameraController cameraController)
    {
        this.pool = pool;
        this.cameraController = cameraController;
    }

    public IPlatformController CreatePlatform(Vector2 position, float width, Relative relative)
    {
        PlatformController platform = pool.Spawn();

        float height = cameraController.Borders.TransformPointY(position.y, Relative.Bottom);

        Vector2 size = new(width, height);
        platform.SetSize(size);

        position = platform.Borders.TransformPoint(position, relative);
        platform.SetPosition(position);

        return platform;
    }
}