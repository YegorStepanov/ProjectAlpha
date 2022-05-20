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

        float height = _cameraController.Borders.GetRelativePointY(position.y, Relative.Bottom);

        platform.SetSize(new Vector2(width, height));
        platform.SetPosition(position, relative);

        return platform;
    }
}