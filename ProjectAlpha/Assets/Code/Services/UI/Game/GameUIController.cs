using Code.Services.Data;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Services.UI;

public sealed class GameUIController
{
    private readonly GameUIView _gameUIView;
    private readonly IProgress _progress;

    public GameUIController(GameUIView gameUIView, IProgress progress)
    {
        _gameUIView = gameUIView;
        _progress = progress;
    }

    public void ShowUI()
    {
        Impl().Forget();

        async UniTaskVoid Impl()
        {
            _gameUIView.ShowScore();

            await UniTask.Delay(1000);
            _gameUIView.ShowHelp();
        }
    }

    public void HideScore() =>
        _gameUIView.HideScore();

    public void UpdateScore()
    {
        int score = _progress.Session.Score;
        _gameUIView.UpdateScore(score);

        if (score == 1)
            _gameUIView.HideHelp();
    }

    public void UpdateCherries()
    {
        int cherries = _progress.Persistant.Cherries;
        _gameUIView.UpdateCherries(cherries);
    }

    public void HitRedPoint(Vector2 notificationPosition)
    {
        _progress.Session.IncreaseScore();
        _gameUIView.OnRedPointHit(notificationPosition);
    }

    public void ShowGameOver()
    {
        //earthshake screen
        _progress.Session.IncreaseRestartNumber();
        _gameUIView.ShowGameOver();
    }

    public void HideGameOver()
    {
        _gameUIView.HideGameOver();
    }
}
