using Code.Services.Game.UI;
using Code.VContainer;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Services;

public sealed class PlatformSpawner : Spawner<Platform>
{
    private readonly Camera _camera;
    private readonly Settings _settings;
    private readonly IWidthGenerator _widthGenerator;
    private readonly GameWorld _gameWorld;

    public PlatformSpawner(IAsyncPool<Platform> pool, Camera camera, Settings settings, IWidthGenerator widthGenerator, GameWorld gameWorld)
        : base(pool)
    {
        _camera = camera;
        _settings = settings;
        _widthGenerator = widthGenerator;
        _gameWorld = gameWorld;
    }

    public UniTask<IPlatform> CreateMenuPlatformAsync()
    {
        float width = _settings.MenuPlatformWidth;
        float posX = _camera.ViewportToWorldPositionX(_settings.ViewportMenuPlatformPositionX);
        return CreateAsync(posX, width, Relative.Center, false);
    }

    public UniTask<IPlatform> CreatePlatformAsync(float posX, Relative relative, bool redPointEnabled = true)
    {
        float width = _widthGenerator.NextWidth();
        return CreateAsync(posX, width, relative, redPointEnabled);
    }

    private UniTask<IPlatform> CreateAsync(float posX, float width, Relative relative, bool redPointEnabled)
    {
        Vector2 position = new(posX, _gameWorld.CurrentPositionY);
        Vector2 size = new(width, _gameWorld.PlatformHeight);
        return CreateAsync(position, size, relative, redPointEnabled);
    }

    private async UniTask<IPlatform> CreateAsync(Vector2 position, Vector2 size, Relative relative, bool redPointEnabled)
    {
        Platform platform = await SpawnAsync();
        platform.SetSize(size);
        platform.SetPosition(position, relative);
        platform.RedPoint.Toggle(redPointEnabled);
        return platform;
    }

    [System.Serializable]
    public class Settings
    {
        public float ViewportMenuPlatformPositionX = 0.5f;
        public float MenuPlatformWidth = 2f;
    }
}
