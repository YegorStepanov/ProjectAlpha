using UnityEngine;

namespace Code.Services;

public sealed class PlatformSpawner
{
    private readonly CameraController _cameraController;
    private readonly PlatformController.Pool _pool;

    public PlatformSpawner(PlatformController.Pool pool, CameraController cameraController)
    {
        _pool = pool;
        _cameraController = cameraController;
    }

    public IPlatformController CreatePlatform(Vector2 position, float width, Relative relative)
    {
        PlatformController platform = _pool.Spawn();

        float height = _cameraController.Borders.TransformPointY(position.y, Relative.Bottom);

        Vector2 size = new(width, height);
        platform.SetSize(size);

        position = platform.Borders.TransformPoint(position, relative);
        platform.SetPosition(position);

        return platform;
    }
}