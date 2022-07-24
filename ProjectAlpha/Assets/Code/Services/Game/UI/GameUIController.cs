using UnityEngine;

namespace Code.Services.Game.UI;

public sealed class GameUIController
{
    private readonly GameUI _gameUI;
    private readonly IProgress _progress;

    public GameUIController(GameUI gameUI, IProgress progress)
    {
        _gameUI = gameUI;
        _progress = progress;
    }

    public void UpdateScore()
    {
        int score = _progress.Session.Score;
        _gameUI.UpdateScore(score);

        if (score == 0) //-1? or <0
            _gameUI.ShowHelp();

        if (score >= 1)
            _gameUI.HideHelp();
    }

    public void UpdateCherries()
    {
        int cherries = _progress.Persistant.Cherries;
        _gameUI.UpdateCherries(cherries);
    }

    public void OnRedPointHit(Vector2 notificationPosition)
    {
        _progress.Session.IncreaseScore();
        _gameUI.OnRedPointHit(notificationPosition);
    }

    public void ShowGameOver()
    {
        _progress.Session.IncreaseRestartNumber();
        _gameUI.ShowGameOver();
    }

    public void HideGameOver()
    {
        _gameUI.HideGameOver();
    }
}
