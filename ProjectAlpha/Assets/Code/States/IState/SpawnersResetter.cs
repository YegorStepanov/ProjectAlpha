using Code.Services;

namespace Code.States;

public class SpawnersResetter
{
    private readonly PlatformSpawner _platformSpawner;
    private readonly CherrySpawner _cherrySpawner;
    private readonly StickSpawner _stickSpawner;

    public SpawnersResetter(PlatformSpawner platformSpawner, CherrySpawner cherrySpawner, StickSpawner stickSpawner)
    {
        _platformSpawner = platformSpawner;
        _cherrySpawner = cherrySpawner;
        _stickSpawner = stickSpawner;
    }

    public void DespawnAll()
    {
        _platformSpawner.DespawnAll();
        _cherrySpawner.DespawnAll();
        _stickSpawner.DespawnAll();
    }
}
