using Code.Services.Data;
using Code.Services.Spawners;

namespace Code.Services
{
    public sealed class GameStateResetter
    {
        private readonly SpawnersResetter _spawnersResetter;
        private readonly IProgress _progress;

        public GameStateResetter(SpawnersResetter spawnersResetter, IProgress progress)
        {
            _spawnersResetter = spawnersResetter;
            _progress = progress;
        }

        public void ResetState()
        {
            _spawnersResetter.DespawnAll();
            _progress.Session.ResetScore();
        }
    }
}