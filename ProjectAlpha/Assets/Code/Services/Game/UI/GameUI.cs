﻿using System.Threading;
using Code.Animations.Game;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using VContainer;

namespace Code.Services.Game.UI;
//change

public sealed class GameUI : MonoBehaviour //IDisposable
{
    [SerializeField] private ChangeScoreAnimation _changeScoreAnimation;
    [SerializeField] private ChangeCherryCountAnimation _changeCherryCountAnimation;
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

    public void UpdateScore(int score)
    {
        //rename Show -> Play
        _changeScoreAnimation.Show(score, animate: score != 0);
    }

    public void UpdateCherryCount(int cherryCount)
    {
        _changeCherryCountAnimation.Show(cherryCount);
    }

    public void OnRedPointHit(Vector2 position)
    {
        _redPointHitAnimation.ShowAsync(_token).Forget();
        _redPointHitGameAnimation.ShowAsync(position, _token).Forget();
    }

    public void ShowHelp() =>
        _showStartHelpAnimation.Show();

    public void HideHelp() =>
        _showStartHelpAnimation.HideAsync(_token).Forget();

    public void ShowGameOver() =>
        _gameOverCanvas.gameObject.SetActive(true);

    public void HideGameOver() =>
        _gameOverCanvas.gameObject.SetActive(false);
}

//create issues:
//disallow nulls
//add NonLazy()
//prefab.name
//open internal interfaces