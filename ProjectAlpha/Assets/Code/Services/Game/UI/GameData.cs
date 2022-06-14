using VContainer.Unity;

namespace Code.Services.Game.UI;

public sealed class GameData : IStartable
{
    private readonly Camera _camera;
    private readonly Settings _settings;

    public float GameHeight { get; private set; }

    public GameData(Camera camera, Settings settings)
    {
        _camera = camera;
        _settings = settings;
    }

    void IStartable.Start()
    {
        //it should be an camera event
        GameHeight = _camera.ViewportToWorldPositionY(_settings.ViewportMenuHeight);
    }

    [System.Serializable]
    public class Settings
    {
        public float ViewportMenuHeight = 0.2f;
        public float ViewportGameHeight = 0.3f;
    }
}