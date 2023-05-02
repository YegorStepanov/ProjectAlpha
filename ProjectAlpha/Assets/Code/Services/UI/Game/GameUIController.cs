using Code.Services.Data;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Services.UI
{
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
                _gameUIView.ShowCherryNumber();

                await UniTask.Delay(1000);
                _gameUIView.ShowHelpAsync();
            }
        }

        public void HideUI()
        {
            _gameUIView.HideGameOver();
            _gameUIView.HideScore();
            _gameUIView.HideCherryNumber();
            _gameUIView.HideHelp();
            _gameUIView.HideRedPointHit();
        }

        public void UpdateScore()
        {
            int score = _progress.Session.Score;
            _gameUIView.UpdateScoreAsync(score);
        }

        public void HideHelp()
        {
            _gameUIView.HideHelpAsync();
        }

        public void UpdateCherries()
        {
            int cherries = _progress.Persistant.Cherries;
            _gameUIView.UpdateCherriesAsync(cherries);
        }

        public void HitRedPoint(Vector2 notificationPosition)
        {
            _progress.Session.IncreaseScore();
            _gameUIView.OnRedPointHit(notificationPosition);
        }

        public void ShowGameOver()
        {
            _progress.Session.IncreaseRestartNumber();
            _gameUIView.ShowGameOver();
        }
    }
}