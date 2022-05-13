using Code.VContainer;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Services;

public sealed class PlatformSpawner
{
    private readonly IAsyncRecyclablePool<PlatformController> _pool;
    private readonly CameraController _cameraController;

    public PlatformSpawner(IAsyncRecyclablePool<PlatformController> pool, CameraController cameraController)
    {
        _pool = pool;
        _cameraController = cameraController;
    }

    public async UniTask<IPlatformController> CreatePlatformAsync(Vector2 position, float width, Relative relative)
    {
        PlatformController platform = await _pool.SpawnAsync();
        
        float height = _cameraController.Borders.TransformPointY(position.y, Relative.Bottom);

        Vector2 size = new(width, height);
        platform.SetSize(size);

        position = platform.Borders.TransformPoint(position, relative);
        platform.SetPosition(position);

        return platform;
    }
}