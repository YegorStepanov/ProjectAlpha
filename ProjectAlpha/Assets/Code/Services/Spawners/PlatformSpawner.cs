using Code.AddressableAssets;
using Code.Common;
using Code.Data;
using Code.Services.Entities;
using Code.Services.Infrastructure;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Services.Spawners;

public sealed class PlatformSpawner : Spawner<Platform>
{
    private readonly ICamera _camera1;
    private readonly Settings _settings;
    private readonly IPlatformWidthGenerator _platformWidthGenerator;
    private readonly GameWorld _gameWorld;

    public PlatformSpawner(IAsyncPool<Platform> pool, ICamera camera1, Settings settings, IPlatformWidthGenerator platformWidthGenerator, GameWorld gameWorld)
        : base(pool)
    {
        _camera1 = camera1;
        _settings = settings;
        _platformWidthGenerator = platformWidthGenerator;
        _gameWorld = gameWorld;
    }

    public UniTask<IPlatform> CreateMenuPlatformAsync()
    {
        float width = _settings.MenuPlatformWidth;
        float posX = _camera1.ViewportToWorldPositionX(_settings.ViewportMenuPlatformPositionX);
        return CreateAsync(posX, width, Relative.Center, false);
    }

    public UniTask<IPlatform> CreatePlatformAsync(float posX, Relative relative, bool redPointEnabled = true)
    {
        float width = _platformWidthGenerator.NextWidth();
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
        platform.PlatformRedPoint.Toggle(redPointEnabled);
        return platform;
    }

    [System.Serializable]
    public class Settings
    {
        public float ViewportMenuPlatformPositionX = 0.5f;
        public float MenuPlatformWidth = 2f;
    }
}
