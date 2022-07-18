using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Services;

public sealed class HeroSpawner
{
    private readonly Hero _asset;

    public HeroSpawner(Hero asset) =>
        _asset = asset;

    public UniTask<IHero> CreateAsync(Vector2 position, Relative relative)
    {
        _asset.SetPosition(position, relative);
        return UniTask.FromResult<IHero>(_asset);
    }
}
