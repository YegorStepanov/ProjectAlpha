using Sirenix.OdinInspector;

namespace Code.Services.Game.UI;

public sealed class GameData //rename to GameWorld
{
    private readonly Camera _camera;
    private readonly Settings _settings;

    private bool _isGameHeight;

    public float CurrentPositionY => _isGameHeight ? GamePositionY : MenuPositionY;
    public float PlatformHeight => GamePositionY - _camera.Borders.Bot;

    private float GamePositionY => _camera.Borders.Bot + _camera.Borders.Height * _settings.ViewportGamePositionY;
    private float MenuPositionY => _camera.Borders.Bot + _camera.Borders.Height * _settings.ViewportMenuPositionY;

    public GameData(Camera camera, Settings settings)
    {
        _camera = camera;
        _settings = settings;
    }

    public void SwitchToMenuHeight() => _isGameHeight = false;
    public void SwitchToGameHeight() => _isGameHeight = true;

    [System.Serializable]
    public class Settings
    {
        [ValidateInput("@$value <= ViewportGamePositionY", "this must be equal or greater then ViewportGamePositionY")]
        [MinValue(0), MaxValue(1)]
        public float ViewportMenuPositionY = 0.2f;
        [MinValue(0), MaxValue(1)]
        public float ViewportGamePositionY = 0.3f;
    }
}
