using Code.AddressableAssets;
using Code.Common;
using Code.Services.Entities;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Services.Spawners
{
    public sealed class StickSpawner : Spawner<Stick>
    {
        private readonly Settings _settings;

        public StickSpawner(IAsyncPool<Stick> pool, Settings settings) : base(pool) =>
            _settings = settings;

        public float StickWidth => _settings.StickWidth;

        public async UniTask<IStick> CreateAsync(Vector2 position)
        {
            Stick stick = await SpawnAsync();
            stick.ResetStick();

            stick.SetPosition(position, Relative.Center);
            stick.SetWidth(_settings.StickWidth);
            return stick;
        }

        [System.Serializable]
        public class Settings
        {
            public float StickWidth = 0.04f;
        }
    }
}