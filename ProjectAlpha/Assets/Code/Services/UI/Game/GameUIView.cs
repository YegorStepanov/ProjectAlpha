﻿using System.Threading;
using Code.Animations.Game;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using VContainer;

namespace Code.Services.UI;

public sealed class GameUIView : MonoBehaviour
{
    [SerializeField] private ChangeScoreAnimation _changeScoreAnimation;
    [SerializeField] private ChangeCherriesAnimation _changeCherriesAnimation;
    [SerializeField] private ShowStartHelpAnimation _showStartHelpAnimation;
    [SerializeField] private RedPointHitAnimation _redPointHitAnimation;
    [SerializeField] private Canvas _gameOverCanvas;

    private RedPointHitGameAnimation _redPointHitGameAnimation;
    private CancellationToken _token;

    [Inject, UsedImplicitly]
    private void Construct(RedPointHitGameAnimation redPointHitGameAnimation, CancellationToken token)
    {
        _redPointHitGameAnimation = redPointHitGameAnimation;
        _token = token;
    }

    private void Awake()
    {
        HideGameOver();
        ShowHelp();
    }

    public void ShowScore()
    {
        _changeScoreAnimation.gameObject.SetActive(true); //todo -> canvas
    }

    public void HideScore()
    {
        _changeScoreAnimation.gameObject.SetActive(false); //todo -> canvas
    }

    public void UpdateScore(int score)
    {
        _changeScoreAnimation.Play(score, animate: score != 0);
    }

    public void UpdateCherries(int cherries)
    {
        _changeCherriesAnimation.Play(cherries);
    }

    public void OnRedPointHit(Vector2 notificationPosition)
    {
        _redPointHitAnimation.PlayAsync(_token).Forget();
        _redPointHitGameAnimation.PlayAsync(notificationPosition, _token).Forget();
    }

    public void ShowHelp() =>
        _showStartHelpAnimation.Play();

    public void HideHelp() =>
        _showStartHelpAnimation.HideAsync(_token).Forget();

    public void ShowGameOver() =>
        _gameOverCanvas.gameObject.SetActive(true);

    public void HideGameOver() =>
        _gameOverCanvas.gameObject.SetActive(false);
}