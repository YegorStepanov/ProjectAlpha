using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Services;

public sealed class HeroSpawner
{
    private readonly HeroController _asset;

    public HeroSpawner(HeroController asset) =>
        _asset = asset;

    public UniTask<IHeroController> CreateHeroAsync(Vector2 position, Relative relative)
    {
        //remove spawner?
        _asset.TeleportTo(position, relative);
        return UniTask.FromResult<IHeroController>(_asset);
    }
}