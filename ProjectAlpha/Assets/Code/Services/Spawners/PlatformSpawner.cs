using Code.AddressableAssets;
using Code.Common;
using Code.Data;
using Code.Extensions;
using Code.Services.Entities;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Services.Spawners
{
    public sealed class PlatformSpawner : Spawner<Platform>
    {
        private readonly IPlatformWidthGenerator _widthGenerator;
        private readonly ICamera _camera;
        private readonly GameSettings _settings;

        public PlatformSpawner(IAsyncPool<Platform> pool, IPlatformWidthGenerator widthGenerator, ICamera camera, GameSettings settings) : base(pool)
        {
            _widthGenerator = widthGenerator;
            _camera = camera;
            _settings = settings;
        }

        public UniTask<IPlatform> CreateMenuPlatformAsync(float positionY, float height)
        {
            float positionX = _camera.ViewportToWorldPosX(_settings.ViewportMenuPlatformPositionX);

            Vector2 position = new(positionX, positionY);
            Vector2 size = new(_settings.MenuPlatformWidth, height);
            return CreateAsync(position, size, Relative.Top, false);
        }

        public UniTask<IPlatform> CreateRestartPlatformAsync(float positionY, float height)
        {
            Vector2 position = new(_camera.Borders.Left, positionY);
            Vector2 size = new(_settings.MenuPlatformWidth, height);
            return CreateAsync(position, size, Relative.LeftTop, false);
        }

        public UniTask<IPlatform> CreateGamePlatformAsync(Vector2 position, float height)
        {
            Vector2 size = new(_widthGenerator.NextWidth(), height);
            return CreateAsync(position, size, Relative.LeftTop, true);
        }

        private async UniTask<IPlatform> CreateAsync(Vector2 position, Vector2 size, Relative relative, bool isRedPointEnable)
        {
            Platform platform = await SpawnAsync();
            platform.SetSize(size);
            platform.SetPosition(position, relative);
            platform.PlatformRedPoint.ToggleVisibility(isRedPointEnable);
            return platform;
        }
    }
}
