using Cysharp.Threading.Tasks;
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

    public void ShowUI()
    {
        Impl().Forget();

        async UniTaskVoid Impl()
        {
            _gameUI.ShowScore();

            await UniTask.Delay(1000);
            _gameUI.ShowHelp();
        }
    }

    public void HideUI() =>
        _gameUI.HideScore();

    public void UpdateScore()
    {
        int score = _progress.Session.Score;
        _gameUI.UpdateScore(score);

        if (score == 1)
            _gameUI.HideHelp();
    }

    public void UpdateCherries()
    {
        int cherries = _progress.Persistant.Cherries;
        _gameUI.UpdateCherries(cherries);
    }

    public void HitRedPoint(Vector2 notificationPosition)
    {
        _progress.Session.IncreaseScore();
        _gameUI.OnRedPointHit(notificationPosition);
    }

    public void ShowGameOver()
    {
        //earthshake screen
        _progress.Session.IncreaseRestartNumber();
        _gameUI.ShowGameOver();
    }

    public void HideGameOver()
    {
        _gameUI.HideGameOver();
    }
}
