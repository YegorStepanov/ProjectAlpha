using UnityEngine;
using VContainer.Unity;

namespace Code.Services.Game.UI;

public sealed class GameData : IInitializable, IStartable
{
    private const string CherryCountKey = "CherryCount";

    private readonly Camera _camera;
    private readonly Settings _settings;

    public int Score { get; private set; }
    public int CherryCount { get; private set; }
    public float GameHeight { get; private set; }

    public GameData(Camera camera, Settings settings)
    {
        _camera = camera;
        _settings = settings;
    }

    void IInitializable.Initialize()
    {
        Score = -1;
        CherryCount = LoadCherryCount();
    }

    void IStartable.Start()
    {
        //it should be an camera event
        GameHeight = _camera.ViewportToWorldPositionY(_settings.ViewportMenuHeight);
    }

    public void IncreaseScore() =>
        Score++;

    public void IncreaseCherryCount()
    {
        CherryCount++;
        SaveCherryCount(CherryCount);
    }

    private static int LoadCherryCount() =>
        PlayerPrefs.GetInt(CherryCountKey, 0);

    private static void SaveCherryCount(int value) =>
        PlayerPrefs.SetInt(CherryCountKey, value);


    [System.Serializable]
    public class Settings
    {
        public float ViewportMenuHeight = 0.2f;
        public float ViewportGameHeight = 0.3f;
    }
}