using UnityEngine;
using VContainer.Unity;

namespace Code.Services.Game.UI;

public sealed class GameUIController : IStartable
{
    private readonly GameData _gameData;
    private readonly GameUI _gameUI;

    public GameUIController(GameData gameData, GameUI gameUI)
    {
        _gameData = gameData;
        _gameUI = gameUI;
    }

    public void Start()
    {
        _gameUI.ShowHelp();
    }

    public void IncreaseScore()
    {
        _gameData.IncreaseScore();
        _gameUI.UpdateScore(_gameData.Score);

        if (_gameData.Score == 1)
            _gameUI.HideHelp(); //hmm where it go?
    }

    public void IncreaseCherryCount()
    {
        _gameData.IncreaseCherryCount();
        _gameUI.UpdateCherryCount(_gameData.CherryCount);
    }

    public void OnRedPointHit(Vector2 position) //IPlatform?
    {
        IncreaseScore();
        _gameUI.OnRedPointHit(position);
    }

    public void ShowGameOver()
    {
        _gameUI.ShowGameOver();
    }

    public void HideGameOver()
    {
        _gameUI.HideGameOver();
    }
}