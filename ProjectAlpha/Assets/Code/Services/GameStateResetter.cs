using Code.Common;
using Code.Services.Data;
using Code.Services.Infrastructure;
using Code.Services.Spawners;
using Cysharp.Threading.Tasks;

namespace Code.Services;

public sealed class GameStateResetter
{
    private readonly ICamera _camera1;
    private readonly SpawnersResetter _spawnersResetter;
    private readonly IProgress _progress;

    public GameStateResetter(ICamera camera1, SpawnersResetter spawnersResetter, IProgress progress)
    {
        _camera1 = camera1;
        _spawnersResetter = spawnersResetter;
        _progress = progress;
    }

    public async UniTask ResetAsync()
    {
        await using UniTaskDisposable _ = ChangeBackground();
        _camera1.RestoreInitialPosition();
        _spawnersResetter.DespawnAll();
        _progress.Session.ResetScore();
    }

    private UniTask ChangeBackground() =>
        _camera1.ChangeBackgroundAsync();
}
