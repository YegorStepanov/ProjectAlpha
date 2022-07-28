using Code.Services;
using Cysharp.Threading.Tasks;

namespace Code.States;

public class GameStateResetter
{
    private readonly Camera _camera;
    private readonly SpawnersResetter _spawnersResetter;
    private readonly IProgress _progress;

    public GameStateResetter(Camera camera, SpawnersResetter spawnersResetter, IProgress progress)
    {
        _camera = camera;
        _spawnersResetter = spawnersResetter;
        _progress = progress;
    }

    public async UniTask ResetAsync()
    {
        await using UniTaskDisposable _ = ChangeBackground();
        _camera.RestoreInitialPosition();
        _spawnersResetter.DespawnAll();
        _progress.Session.ResetScore();
    }

    private UniTask ChangeBackground() =>
        _camera.ChangeBackgroundAsync();
}
