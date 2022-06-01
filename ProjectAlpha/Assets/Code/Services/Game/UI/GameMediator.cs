using UnityEngine;
using VContainer;

namespace Code.Services.Game.UI;

public sealed class GameMediator
{
    [Inject] GameSceneLoader _gameSceneLoader;
    [Inject] GameUIController _gameUIController;
    
    public GameStateMachine gameStateMachine; //rework later

    public void IncreaseScore() => _gameUIController.IncreaseScore();
    public void IncreaseCherryCount() => _gameUIController.IncreaseCherryCount();
    public void OnRedPointHit(Vector2 position) => _gameUIController.OnRedPointHit(position);
    public void GameOver() => _gameUIController.ShowGameOver(); //earthshake screen
    public void HideGameOver() => _gameUIController.HideGameOver();
    public void LoadMenu() => _gameSceneLoader.LoadMenu();
    public void Restart() => _gameSceneLoader.Restart(gameStateMachine);
}