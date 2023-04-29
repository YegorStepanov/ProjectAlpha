using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Code.Animations.Game
{
    public sealed class ChangeScoreAnimation : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;

        public void Play(int score, bool animate = true)
        {
            _text.text = score.ToString();

            if (animate)
                _text.transform.DOPunchScale(new Vector2(0.3f, 0.3f), 0.3f, 0, 0f);
        }
    }
}