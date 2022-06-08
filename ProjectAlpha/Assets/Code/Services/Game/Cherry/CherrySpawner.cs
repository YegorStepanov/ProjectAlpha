using Code.Services.Game.UI;
using Code.VContainer;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Services;

public sealed class CherrySpawner
{
    private readonly IAsyncPool<Cherry> _pool;
    private readonly GameData _gameData;
    private readonly Settings _settings;

    public CherrySpawner(IAsyncPool<Cherry> pool, GameData gameData, Settings settings)
    {
        _pool = pool;
        _gameData = gameData;
        _settings = settings;
    }
    
    public async UniTask<ICherry> CreateAsync(float posX, Relative relative)
    {
        if (Random.value < _settings.CherryChance)
            return new CherryNull();

        Cherry cherry = await _pool.SpawnAsync();

        cherry.SetPosition(new Vector2(posX, _gameData.GameHeight), relative);
        return cherry;
    }

    //we should disable cherry, hide?
    public void DespawnAll()
    {
        _pool.DespawnAll();
    }

    public void Despawn(Cherry cherry)
    {
        _pool.Despawn(cherry);
    }

    [System.Serializable]
    public class Settings
    {
        [Range(0, 1)] 
        public float CherryChance = 0.1f;
    }
}