using System.Threading;
using Code.Animations.Game;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using VContainer;

namespace Code.Services.Game.UI;
//change

public sealed class GameUIMediator : MonoBehaviour
{
    [SerializeField] private ChangeScoreAnimation _changeScoreAnimation;
    [SerializeField] private ChangeCherryCountAnimation _changeCherryCountAnimation;
    [SerializeField] private ShowStartHelpAnimation _showStartHelpAnimation;
    [SerializeField] private RedPointHitAnimation _redPointHitAnimation;

    private RedPointHitGameAnimation _redPointHitGameAnimation;
    private CancellationToken _token;

    private int _score = -1;
    private int _cherryCount;

    [Inject, UsedImplicitly]
    private void Construct(RedPointHitGameAnimation redPointHitGameAnimation, CancellationToken token)
    {
        _redPointHitGameAnimation = redPointHitGameAnimation;
        _token = token;
    }

    public void IncreaseScore()
    {
        _score++;
        _changeScoreAnimation.Show(_score);
    }

    public void IncreaseCherryCount()
    {
        _cherryCount++;
        _changeCherryCountAnimation.Show(_cherryCount);
    }

    public void ShowRedPointHitNotification(Vector2 position)
    {
        _ = _redPointHitAnimation.ShowAsync(_token);
        _ = _redPointHitGameAnimation.ShowAsync(position, _token);
    }

    public async UniTask ShowHelp()
    {
        _showStartHelpAnimation.Show();

        //wait when score changes to 1
        await UniTask.Delay(3000, cancellationToken: _token);

        await _showStartHelpAnimation.HideAsync(_token);
    }
}

//create issues:
//disallow nulls
//add NonLazy()
//prefab.name
//open internal interfaces