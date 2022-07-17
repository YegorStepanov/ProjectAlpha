namespace Code.Services.Game.UI;

public sealed class GameData //rename to GameWorld
{
    private readonly Camera _camera;
    private readonly Settings _settings;

    private bool _isGameHeight;

    //rename height to PosY?
    public float CurrentHeight => _isGameHeight ? GameHeight : MenuHeight;
    public float GameHeight => _camera.Borders.Bot + _camera.Borders.Height * _settings.ViewportGameHeight;
    public float MenuHeight => _camera.Borders.Bot + _camera.Borders.Height * _settings.ViewportMenuHeight;

    public float PlatformHeight => GameHeight - _camera.Borders.Bot;

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
        //add odin attribute, MenuHeight cannot be more that GameHeight
        //when MenuHeight > GameHeight -> error
        public float ViewportMenuHeight = 0.2f;
        public float ViewportGameHeight = 0.3f;
    }
}
