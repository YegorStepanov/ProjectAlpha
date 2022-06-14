using Code.States;
using UnityEngine;
using VContainer;

namespace Code.Services.Game.UI;

public sealed class GameMediator
{
    [Inject] private GameSceneLoader _gameSceneLoader;
    [Inject] private GameUIController _gameUIController;
    [Inject] private GameProgress _gameProgress;
    [Inject] private PlayerProgress _playerProgress;

    public GameStateMachine gameStateMachine; //rework later

    public void IncreaseScore() => _gameProgress.IncreaseScore();
    // public void ResetScore() => _gameUIController.ResetScore();
    public void IncreaseCherryCount() => _playerProgress.IncreaseCherryCount();
    public void OnRedPointHit(Vector2 position) => _gameUIController.OnRedPointHit(position);
    public void GameOver() => _gameUIController.ShowGameOver(); //earthshake screen
    public void HideGameOver() => _gameUIController.HideGameOver();
    public void LoadMenu() => _gameSceneLoader.LoadMenu();

    public void Restart() //todo
    {
        HideGameOver();
        //black fade out
        //HideGameOver();
        gameStateMachine.Enter<RestartState>();
    }
}