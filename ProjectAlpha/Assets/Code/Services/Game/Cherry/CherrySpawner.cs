using Code.VContainer;
using Cysharp.Threading.Tasks;

namespace Code.Services;

public sealed class CherrySpawner
{
    private readonly IAsyncPool<Cherry> _pool;

    public CherrySpawner(IAsyncPool<Cherry> pool) =>
        _pool = pool;

    public async UniTask<ICherry> CreateAsync(IPlatform nextPlatform)
    {
        Cherry cherry = await _pool.SpawnAsync();

        cherry.SetPosition(nextPlatform.Borders.LeftTop, Relative.RightTop);
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