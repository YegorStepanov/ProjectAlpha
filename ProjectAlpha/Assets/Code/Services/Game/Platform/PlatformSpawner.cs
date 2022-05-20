using System;
using Code.VContainer;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Services;

public sealed class PlatformSpawner
{
    private readonly IAsyncRecyclablePool<PlatformController> _pool;
    private readonly CameraController _cameraController;
    private readonly Settings _settings;
    private readonly WidthGeneratorSpawner _widthGeneratorSpawner;
    private readonly PositionGeneratorSpawner _positionGeneratorSpawner;

    public PlatformSpawner(IAsyncRecyclablePool<PlatformController> pool, CameraController cameraController,
        Settings settings, WidthGeneratorSpawner widthGeneratorSpawner,
        PositionGeneratorSpawner positionGeneratorSpawner)
    {
        _pool = pool;
        _cameraController = cameraController;
        _settings = settings;
        _widthGeneratorSpawner = widthGeneratorSpawner;
        _positionGeneratorSpawner = positionGeneratorSpawner;
    }

    public async UniTask<IPlatformController> CreateMenuPlatformAsync()
    {
        Vector2 position = _cameraController.ViewportToWorldPosition(_settings.viewportMenuPlatformPosition);
        float width = _settings.MenuPlatformWidth;
        return await CreatePlatformAsync(position, width, Relative.Center);
    }

    private async UniTask<IPlatformController> CreatePlatformAsync(Vector2 position, float width, Relative relative)
    {
        PlatformController platform = await _pool.SpawnAsync();

        float height = _cameraController.Borders.GetRelativePointY(position.y, Relative.Bottom);

        platform.SetSize(new Vector2(width, height));
        platform.SetPosition(position, relative);

        return platform;
    }

    public async UniTask<IPlatformController> CreateAndMoveNextPlatformAsync(IPlatformController currentPlatform)
    {
        float width = (await _widthGeneratorSpawner.GetAsync()).NextWidth();

        IPlatformController nextPlatform = await CreateNextPlatformAsync(currentPlatform, width);
        await MoveNextPlatformToRandomPoint(currentPlatform, nextPlatform);
        return nextPlatform;
    }

    private async UniTask<IPlatformController> CreateNextPlatformAsync(IPlatformController currentPlatform, float width)
    {
        float distanceToPlatform = currentPlatform.Borders.Left - _cameraController.Borders.Left;

        Vector2 position = new(
            _cameraController.Borders.Right + distanceToPlatform,
            currentPlatform.Borders.Top);

        return await CreatePlatformAsync(position, width, Relative.Left);
    }

    private async UniTask MoveNextPlatformToRandomPoint(
        IPlatformController currentPlatform,
        IPlatformController nextPlatform)
    {
        IPositionGenerator d = await _positionGeneratorSpawner.GetAsync();

        Debug.Log(d.GetType());
        float newPos = d.NextPosition(currentPlatform, nextPlatform);
        await nextPlatform.MoveAsync(newPos);
        Debug.Log("Current: " + nextPlatform.Position);
        // await UniTask.Delay(300); do not use random here
    }

    [Serializable]
    public class Settings
    {
        public Vector2 viewportMenuPlatformPosition = new(0.5f, 0.2f);

        public float MenuPlatformWidth = 2f;
    }
}