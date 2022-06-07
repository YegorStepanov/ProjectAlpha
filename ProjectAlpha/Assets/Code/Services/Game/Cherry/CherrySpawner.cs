using Code.Services.Game.UI;
using Code.VContainer;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Services;

public sealed class CherrySpawner
{
    private readonly IAsyncPool<Cherry> _pool;
    private readonly GameData _gameData;

    public CherrySpawner(IAsyncPool<Cherry> pool, GameData gameData)
    {
        _pool = pool;
        _gameData = gameData;
    }

    public async UniTask<ICherry> CreateAsync(float posX, Relative relative)
    {
        Cherry cherry = await _pool.SpawnAsync();

        cherry.SetPosition(new Vector2(posX, _gameData.GameHeight), relative);
        return cherry;
    }

    public void DespawnAll()
    {
        _pool.DespawnAll();
    }

    public void Despawn(Cherry cherry)
    {
        _pool.Despawn(cherry);
    }
}