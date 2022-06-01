using System;
using Code.Services.Game.UI;
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
    private readonly GameData _gameData;

    public PlatformSpawner(IAsyncPool<PlatformController> pool, CameraController cameraController,
        Settings settings, IWidthGenerator widthGenerator, GameData gameData)
    {
        _pool = pool;
        _cameraController = cameraController;
        _settings = settings;
        _widthGenerator = widthGenerator;
        _gameData = gameData;
    }

    public UniTask<IPlatformController> CreateMenuPlatformAsync()
    {
        float width = _settings.MenuPlatformWidth;
        float posX = _cameraController.ViewportToWorldPositionX(_settings.ViewportMenuPlatformPositionX);
        return CreatePlatformAsync(posX, width, Relative.Center, false);
    }

    public UniTask<IPlatformController> CreatePlatformAsync(float posX, Relative relative, bool redPointEnabled = true) //Relative.Left
    {
        float width = _widthGenerator.NextWidth();
        return CreatePlatformAsync(posX, width, relative, redPointEnabled);
    }

    public void DespawnAll() =>
        _pool.DespawnAll();

    private async UniTask<IPlatformController> CreatePlatformAsync(float posX, float width, Relative relative, bool redPointEnabled)
    {
        Vector2 position = new Vector2(posX, _gameData.GameHeight);
        
        PlatformController platform = await _pool.SpawnAsync();

        float height = position.y + _cameraController.Borders.Height / 2f;

        platform.SetSize(new Vector2(width, height));
        platform.SetPosition(position, relative);
        platform.ToggleRedPoint(redPointEnabled);

        return platform;
    }

    [Serializable]
    public class Settings
    {
        public float ViewportMenuPlatformPositionX = 0.5f;
        public float MenuPlatformWidth = 2f;
    }
}