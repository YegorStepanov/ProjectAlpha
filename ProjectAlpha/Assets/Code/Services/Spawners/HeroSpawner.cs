using Code.AddressableAssets;
using Code.Common;
using Code.Services.Entities;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Services.Spawners
{
    public sealed class HeroSpawner : Spawner<Hero>
    {
        public HeroSpawner(IAsyncPool<Hero> pool) : base(pool) { }

        public async UniTask<IHero> CreateAsync(Vector2 position, Relative relative)
        {
            Hero hero = await SpawnAsync();
            FlipUp(hero);
            hero.SetPosition(position, relative);
            return hero;
        }

        private static void FlipUp(Hero hero)
        {
            if (hero.IsFlipped)
                hero.Flip();
        }
    }
}