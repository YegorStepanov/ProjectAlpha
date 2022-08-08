using Code.AddressableAssets;
using Code.Common;
using Code.Services.Entities;
using Code.Services.Infrastructure;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Services.Spawners;

public sealed class CherrySpawner : Spawner<Cherry>, ICherryPickHandler
{
    private readonly IRandomizer _randomizer;
    private readonly GameSettings _settings;

    public CherrySpawner(IAsyncPool<Cherry> pool, IRandomizer randomizer, GameSettings settings)
        : base(pool)
    {
        _randomizer = randomizer;
        _settings = settings;
    }

    public UniTask<ICherry> CreateAsync(Vector2 position, Relative relative)
    {
        if (_randomizer.NextProbability() > _settings.CherryChance)
            return UniTask.FromResult<ICherry>(CherryNull.Default);

        return Create(position, relative);
    }

    private async UniTask<ICherry> Create(Vector2 position, Relative relative)
    {
        Cherry cherry = await SpawnAsync();
        cherry.SetPosition(position, relative);
        return cherry;
    }

    void ICherryPickHandler.OnCherryPicked(Cherry cherry) => Despawn(cherry);
}
