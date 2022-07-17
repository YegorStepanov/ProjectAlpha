﻿using Code.Services.Game.UI;
using Code.VContainer;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Services;

public sealed class CherrySpawner : ICherryPickHandler
{
    private readonly IAsyncPool<Cherry> _pool;
    private readonly GameData _gameData;
    private readonly IRandomizer _randomizer;
    private readonly Settings _settings;
    private readonly CherryNull _cherryNull = new();

    public CherrySpawner(IAsyncPool<Cherry> pool, GameData gameData, IRandomizer randomizer, Settings settings)
    {
        _pool = pool;
        _gameData = gameData;
        _randomizer = randomizer;
        _settings = settings;
    }

    public async UniTask<ICherry> CreateAsync(float posX, Relative relative)
    {
        if (_randomizer.NextProbability() > _settings.CherryChance)
            return _cherryNull;

        Cherry cherry = await _pool.SpawnAsync();

        cherry.SetPosition(new Vector2(posX, _gameData.CurrentHeight), relative);
        return cherry;
    }

    //we should disable cherry, hide?
    public void DespawnAll() =>
        _pool.DespawnAll();

    void ICherryPickHandler.OnCherryPicked(Cherry cherry) =>
        _pool.Despawn(cherry);

    [System.Serializable]
    public class Settings
    {
        [Range(0, 1)]
        public float CherryChance = 0.1f;
    }
}
