using Code.AddressableAssets;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Services;

public sealed class HeroSpawner
{
    private readonly IAsyncObject<HeroController> _asset;

    public HeroSpawner(IAsyncObject<HeroController> asset) =>
        _asset = asset;

    public async UniTask<IHeroController> CreateHeroAsync(Vector2 position, Relative relative)
    {
        HeroController hero = await _asset.GetAssetAsync();
        hero.TeleportTo(position, relative);
        return hero;
    }
}