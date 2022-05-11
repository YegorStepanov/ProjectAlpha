using System.Collections.Generic;
using Code.VContainer;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Services;

public sealed class PlatformSpawner
{
    private readonly AddressablePool<PlatformController> _pool;
    private readonly CameraController _cameraController;

    private readonly List<PlatformController> _platforms;

    private int _platformIndex;

    public PlatformSpawner(AddressablePool<PlatformController> pool, CameraController cameraController)
    {
        _pool = pool;
        _cameraController = cameraController;
        _platforms = new List<PlatformController>(_pool.Capacity);
        _platformIndex = 0;
    }

    public async UniTask<IPlatformController> CreatePlatformAsync(Vector2 position, float width, Relative relative)
    {
        (PlatformController platform, bool success) = await _pool.SpawnAsync();
        
        if(success)
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