using Code.VContainer;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Services;

public sealed class CherrySpawner
{
    private readonly IAsyncPool<CherryController> _pool;

    public CherrySpawner(IAsyncPool<CherryController> pool) =>
        _pool = pool;

    public async UniTask<ICherryController> CreateCherryAsync(IPlatformController nextPlatform)
    {
        CherryController cherry = await _pool.SpawnAsync();

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
    
    public void Despawn(CherryController cherry)
    {
        _pool.Despawn(cherry);
    }
}