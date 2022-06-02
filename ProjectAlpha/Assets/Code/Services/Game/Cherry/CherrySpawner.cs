using Code.VContainer;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Services;

public sealed class CherrySpawner
{
    private readonly IAsyncPool<Cherry> _pool;

    public CherrySpawner(IAsyncPool<Cherry> pool) =>
        _pool = pool;

    public async UniTask<ICherry> CreateCherryAsync(IPlatform nextPlatform)
    {
        Cherry cherry = await _pool.SpawnAsync();

        Vector2 position = new(
            nextPlatform.Borders.Left,
            nextPlatform.Borders.Top);

        cherry.TeleportTo(position, Relative.RightTop);
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