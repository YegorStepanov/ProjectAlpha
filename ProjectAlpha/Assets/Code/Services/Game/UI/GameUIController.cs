using System;
using UnityEngine;
using VContainer.Unity;

namespace Code.Services.Game.UI;

public sealed class GameUIController : IStartable, IDisposable
{
    private readonly GameUI _gameUI;
    private readonly GameProgress _gameProgress;
    private readonly PlayerProgress _playerProgress;

    public GameUIController(GameUI gameUI, GameProgress gameProgress, PlayerProgress playerProgress)
    {
        _gameUI = gameUI;
        _gameProgress = gameProgress;
        _playerProgress = playerProgress;

        _gameProgress.ScoreChanged += OnScoreChanged;
        _playerProgress.CherryCountChanged += OnCherryCountChanged;
    }

    public void Dispose()
    {
        _gameProgress.ScoreChanged -= OnScoreChanged;
        _playerProgress.CherryCountChanged -= OnCherryCountChanged;
    }

    void IStartable.Start()
    {
        _gameUI.ShowHelp();
        _gameUI.UpdateScore(_gameProgress.Score);
        _gameUI.UpdateCherryCount(_playerProgress.CherryCount);
    }

    private void OnScoreChanged(int score)
    {
        _gameUI.UpdateScore(score);

        if (score >= 1)
            _gameUI.HideHelp();
    }

    private void OnCherryCountChanged(int cherryCount)
    {
        _gameUI.UpdateCherryCount(cherryCount);
    }

    public void OnRedPointHit(Vector2 notificationPosition)
    {
        _gameProgress.IncreaseScore();
        _gameUI.OnRedPointHit(notificationPosition);
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
