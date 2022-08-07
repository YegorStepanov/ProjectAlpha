using Code.Services.Data;
using Code.Services.Infrastructure;
using Code.Services.Spawners;

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

    public void ResetState()
    {
        _camera1.RestoreInitialPosX();
        _spawnersResetter.DespawnAll();
        _progress.Session.ResetScore();
    }
}
