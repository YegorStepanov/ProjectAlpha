using System;

namespace Code.Services.Game.UI;

public sealed class GameData
{
    private readonly Camera _camera;
    private readonly Settings _settings;
    
    public int Score { get; private set; } = -1;
    public int CherryCount { get; private set; }
    public float GameHeight { get; private set; }

    public GameData(Camera camera, Settings settings)
    {
        _camera = camera;
        _settings = settings;
    }
    
    public void IncreaseScore() =>
        Score++;

    public void IncreaseCherryCount() =>
        CherryCount++;

    public void SetGameHeight() =>
        GameHeight = _camera.ViewportToWorldPositionY(_settings.ViewportGameHeight);

    //todo
    public void SetMenuHeight() =>
        GameHeight = _camera.ViewportToWorldPositionY(_settings.ViewportMenuHeight);
    
    [Serializable]
    public class Settings
    {
        public float ViewportMenuHeight = 0.2f;
        public float ViewportGameHeight = 0.3f;
    }
}