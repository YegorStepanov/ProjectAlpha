using System.Threading;
using Code.Animations.Game;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace Code.Services.UI
{
    public sealed class GameUIView : MonoBehaviour
    {
        [SerializeField] private Canvas _changeScoreCanvas;
        [SerializeField] private ChangeScoreAnimation _changeScoreAnimation;
        [SerializeField] private ChangeCherriesAnimation _changeCherriesAnimation;
        [SerializeField] private ShowStartHelpAnimation _showStartHelpAnimation;
        [SerializeField] private RedPointHitAnimation _redPointHitAnimation;
        [SerializeField] private Canvas _gameOverCanvas;
        [SerializeField] private Canvas _cherryNumberCanvas;
        [SerializeField] private Canvas _redPointHitCanvas;

        private RedPointHitGameAnimation _redPointHitGameAnimation;
        private CancellationToken _token;

        [Inject]
        private void Construct(RedPointHitGameAnimation redPointHitGameAnimation, CancellationToken token)
        {
            _redPointHitGameAnimation = redPointHitGameAnimation;
            _token = token;
        }

        public void ShowScore()
        {
            _changeScoreCanvas.enabled = true;
        }

        public void HideScore()
        {
            _changeScoreCanvas.enabled = false;
        }

        public void UpdateScoreAsync(int score)
        {
            _changeScoreAnimation.Play(score, animate: score != 0);
        }

        public void UpdateCherriesAsync(int cherries)
        {
            _changeCherriesAnimation.Play(cherries);
        }

        public void OnRedPointHit(Vector2 notificationPosition)
        {
            _redPointHitAnimation.PlayAsync(_token).Forget();
            _redPointHitGameAnimation.PlayAsync(notificationPosition, _token).Forget();
        }

        public void ShowHelpAsync() =>
            _showStartHelpAnimation.PlayAsync().Forget();

        public void HideHelpAsync() =>
            _showStartHelpAnimation.HideAsync(_token).Forget();

        public void HideHelp() =>
            _showStartHelpAnimation.Hide();

        public void ShowGameOver() =>
            _gameOverCanvas.enabled = true;

        public void HideGameOver() =>
            _gameOverCanvas.enabled = false;

        public void ShowCherryNumber() =>
            _cherryNumberCanvas.enabled = true;

        public void HideCherryNumber() =>
            _cherryNumberCanvas.enabled = false;

        public void ShowRedPointHit() =>
            _redPointHitCanvas.enabled = true;

        public void HideRedPointHit() =>
            _redPointHitCanvas.enabled = false;
    }
}