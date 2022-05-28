using System;
using Code.VContainer;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Services;

public sealed class PlatformSpawner
{
    private readonly IAsyncPool<PlatformController> _pool;
    private readonly CameraController _cameraController;
    private readonly Settings _settings;
    private readonly IWidthGenerator _widthGenerator;

    public PlatformSpawner(IAsyncPool<PlatformController> pool, CameraController cameraController,
        Settings settings, IWidthGenerator widthGenerator)
    {
        _pool = pool;
        _cameraController = cameraController;
        _settings = settings;
        _widthGenerator = widthGenerator;
    }

    public async UniTask<IPlatformController> CreateMenuPlatformAsync()
    {
        Vector2 position = _cameraController.ViewportToWorldPosition(_settings.viewportMenuPlatformPosition);
        float width = _settings.MenuPlatformWidth;

        IPlatformController platform = await CreatePlatformAsync(position, width, Relative.Center, false);
        return platform;
    }

    private async UniTask<IPlatformController> CreatePlatformAsync(Vector2 position, float width, Relative relative, bool isRedPointEnabled)
    {
        PlatformController platform = await _pool.SpawnAsync();

        float height = _cameraController.Borders.GetRelativePointY(position.y, Relative.Bottom);

        platform.SetSize(new Vector2(width, height));
        platform.SetPosition(position, relative);
        platform.ToggleRedPoint(isRedPointEnabled);

        return platform;
    }

    public async UniTask<IPlatformController> CreateNextPlatformAsync(IPlatformController currentPlatform)
    {
        float width = _widthGenerator.NextWidth();

        IPlatformController nextPlatform = await CreateNextPlatformAsync(currentPlatform, width);
        return nextPlatform;
    }

    private async UniTask<IPlatformController> CreateNextPlatformAsync(IPlatformController currentPlatform, float width)
    {
        float distanceWhichCameraWillMove = currentPlatform.Borders.Left - _cameraController.Borders.Left;

        Vector2 position = new(
            _cameraController.Borders.Right + distanceWhichCameraWillMove,
            currentPlatform.Borders.Top);

        return await CreatePlatformAsync(position, width, Relative.Left, true);
    }

    [Serializable]
    public class Settings
    {
        public Vector2 viewportMenuPlatformPosition = new(0.5f, 0.2f);

        public float MenuPlatformWidth = 2f;
    }
}