using Code.VContainer;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Services;

public sealed class CherrySpawner
{
    private readonly IAsyncPool<CherryController> _pool;
    private readonly CameraController _cameraController;

    public CherrySpawner(IAsyncPool<CherryController> pool, CameraController cameraController)
    {
        _pool = pool;
        _cameraController = cameraController;
    }

    public async UniTask<ICherryController> CreateCherryAsync(IPlatformController currentPlatform,
        IPlatformController nextPlatform)
    {
        CherryController cherry = await _pool.SpawnAsync();

        float distanceWhichCameraWillMove = currentPlatform.Borders.Left - _cameraController.Borders.Left;

        Vector2 position = new(
            _cameraController.Borders.Right + distanceWhichCameraWillMove,
            nextPlatform.Borders.Top);

        cherry.TeleportTo(position, Relative.LeftTop);
        return cherry;
    }
}