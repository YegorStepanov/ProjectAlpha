using Code.Services.Infrastructure;
using Sirenix.OdinInspector;

namespace Code.Services;

public sealed class GameWorld
{
    private readonly ICamera _camera1;
    private readonly Settings _settings;

    private bool _isGameHeight;

    public float CurrentPositionY => _isGameHeight ? GamePositionY : MenuPositionY;
    public float PlatformHeight => GamePositionY - _camera1.Borders.Bot;

    private float GamePositionY => _camera1.Borders.Bot + _camera1.Borders.Height * _settings.ViewportGamePositionY;
    private float MenuPositionY => _camera1.Borders.Bot + _camera1.Borders.Height * _settings.ViewportMenuPositionY;

    public GameWorld(ICamera camera1, Settings settings)
    {
        _camera1 = camera1;
        _settings = settings;
    }

    public void SwitchToMenuHeight() => _isGameHeight = false;
    public void SwitchToGameHeight() => _isGameHeight = true;

    [System.Serializable]
    public class Settings
    {
        [ValidateInput("@$value <= ViewportGamePositionY", "value must be equal or greater then ViewportGamePositionY")]
        [MinValue(0), MaxValue(1)]
        public float ViewportMenuPositionY = 0.2f;
        [MinValue(0), MaxValue(1)]
        public float ViewportGamePositionY = 0.3f;
    }
}
