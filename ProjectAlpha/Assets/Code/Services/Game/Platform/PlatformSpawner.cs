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

    public UniTask<IPlatformController> CreateMenuPlatformAsync()
    {
        Vector2 position = _cameraController.ViewportToWorldPosition(_settings.viewportMenuPlatformPosition);
        float width = _settings.MenuPlatformWidth;
        return CreatePlatformAsync(position, width, Relative.Center, false);
    }

    public UniTask<IPlatformController> CreateNextPlatformAsync(Vector2 position)
    {
        float width = _widthGenerator.NextWidth();
        return CreatePlatformAsync(position, width, Relative.Left, true);
    }

    public void DespawnAll() =>
        _pool.DespawnAll();

    private async UniTask<IPlatformController> CreatePlatformAsync(Vector2 position, float width, Relative relative, bool isRedPointEnabled)
    {
        PlatformController platform = await _pool.SpawnAsync();

        float height = position.y + _cameraController.Borders.Height / 2f;

        platform.SetSize(new Vector2(width, height));
        platform.SetPosition(position, relative);
        platform.ToggleRedPoint(isRedPointEnabled);

        return platform;
    }

    [Serializable]
    public class Settings
    {
        public Vector2 viewportMenuPlatformPosition = new(0.5f, 0.2f);

        public float MenuPlatformWidth = 2f;
    }
}