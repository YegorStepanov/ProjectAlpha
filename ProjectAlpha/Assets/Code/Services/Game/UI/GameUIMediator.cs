using System.Threading;
using Code.Animations.Game;
using Code.States;
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
    [SerializeField] private Canvas _gameOverCanvas;

    private RedPointHitGameAnimation _redPointHitGameAnimation;
    private CancellationToken _token;

    private int _score = -1;
    private int _cherryCount;
    private ISceneLoader _sceneLoader;
    
    public GameStateMachine gameStateMachine; //rework later

    [Inject, UsedImplicitly]
    private void Construct(RedPointHitGameAnimation redPointHitGameAnimation, CancellationToken token, ISceneLoader sceneLoader)
    {
        _redPointHitGameAnimation = redPointHitGameAnimation;
        _sceneLoader = sceneLoader;
        _token = token;
        //_gameStateMachine = gameStateMachine;
    }

    private void Awake() =>
        HideGameOver();

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

    public void ShowGameOver() =>
        _gameOverCanvas.gameObject.SetActive(true);

    public void HideGameOver() =>
        _gameOverCanvas.gameObject.SetActive(false);

    
    public void LoadMenu()
    {
        Core().Forget();

        async UniTaskVoid Core()
        {
            await _sceneLoader.LoadAsync<BootstrapScene>(_token); //it should be black fade out, not red
            // await _sceneLoader.LoadAsync<MenuScene>(_token);
            await _sceneLoader.UnloadAsync<GameScene>(_token);
            // await _sceneLoader.LoadAsync<GameScene>(default);
        }
    }
    
    public void Restart()
    {
        //black fade out
        HideGameOver();
        gameStateMachine.Enter<BootstrapState>();
    }
}

//create issues:
//disallow nulls
//add NonLazy()
//prefab.name
//open internal interfaces