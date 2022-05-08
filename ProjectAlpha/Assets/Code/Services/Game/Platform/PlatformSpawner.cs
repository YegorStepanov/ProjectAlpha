using System.Collections.Generic;
using Code.VContainer;
using UnityEngine;

namespace Code.Services;

public sealed class PlatformSpawner
{
    private readonly MonoBehaviourPool<PlatformController> _pool;
    private readonly CameraController _cameraController;

    private readonly List<PlatformController> _platforms;

    private int _platformIndex;

    public PlatformSpawner(MonoBehaviourPool<PlatformController> pool, CameraController cameraController)
    {
        _pool = pool;
        _cameraController = cameraController;
        _platforms = new List<PlatformController>(_pool.Count);
        _platformIndex = 0;
    }

    public IPlatformController CreatePlatform(Vector2 position, float width, Relative relative)
    {
        if (_pool.TrySpawn(out PlatformController platform))
            _platforms.Add(platform);
        else
        {
            platform = _platforms[_platformIndex];
            _platformIndex = (_platformIndex + 1) % _platforms.Count;
        }

        float height = _cameraController.Borders.TransformPointY(position.y, Relative.Bottom);

        Vector2 size = new(width, height);
        platform.SetSize(size);

        position = platform.Borders.TransformPoint(position, relative);
        platform.SetPosition(position);

        return platform;
    }
}