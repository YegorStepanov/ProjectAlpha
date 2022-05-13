using Code.VContainer;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Services;

public sealed class HeroSpawner
{
    private readonly IAsyncRecyclablePool<HeroController> _pool;

    public HeroSpawner(IAsyncRecyclablePool<HeroController> pool) =>
        _pool = pool;

    public async UniTask<IHeroController> CreateHeroAsync(Vector2 position, Relative relative)
    {
        HeroController hero = await _pool.SpawnAsync();
        hero.TeleportTo(position, relative);
        return hero;
    }
}