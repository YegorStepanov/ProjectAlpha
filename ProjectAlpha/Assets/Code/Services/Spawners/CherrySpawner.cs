using Code.AddressableAssets;
using Code.Common;
using Code.Services.Entities;
using Code.Services.Infrastructure;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Services.Spawners;

public sealed class CherrySpawner : Spawner<Cherry>, ICherryPickHandler
{
    private readonly GameWorld _gameWorld;
    private readonly IRandomizer _randomizer;
    private readonly Settings _settings;
    private readonly CherryNull _cherryNull = new();

    public CherrySpawner(IAsyncPool<Cherry> pool, GameWorld gameWorld, IRandomizer randomizer, Settings settings)
        : base(pool)
    {
        _gameWorld = gameWorld;
        _randomizer = randomizer;
        _settings = settings;
    }

    public async UniTask<ICherry> CreateAsync(float posX, Relative relative)
    {
        if (_randomizer.NextProbability() > _settings.CherryChance)
            return _cherryNull;

        Cherry cherry = await SpawnAsync();

        cherry.SetPosition(new Vector2(posX, _gameWorld.CurrentPositionY), relative);
        return cherry;
    }

    void ICherryPickHandler.OnCherryPicked(Cherry cherry) => Despawn(cherry);

    [System.Serializable]
    public class Settings
    {
        [Range(0, 1)]
        public float CherryChance = 0.1f;
    }
}
