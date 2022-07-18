using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Animations.Game;

public sealed class ChangeCherryCountAnimation : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Image _panel;

    public void Play(int cherryCount)
    {
        _text.text = cherryCount.ToString();
        _panel.transform.DOPunchScale(new Vector2(0.3f, 0.3f), 0.3f, 0, 0f);
    }
}
