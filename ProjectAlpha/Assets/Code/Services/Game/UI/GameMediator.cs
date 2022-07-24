using Code.States;
using UnityEngine;
using UnityEngine.iOS;
using VContainer;

namespace Code.Services.Game.UI;

//mb remove or add a lot of method?
public sealed class GameMediator
{
    [Inject] private GameSceneLoader _gameSceneLoader;
    [Inject] private GameUIController _gameUIController;
    [Inject] private IProgress _progress;

    public GameStateMachine gameStateMachine; //rework later

    public void RequestStoreReview() => Device.RequestStoreReview();
    public void IncreaseScore() => _progress.Session.IncreaseScore();
    public void ResetScore() => _progress.Session.ResetScore();
    public void AddCherry() => _progress.Persistant.AddCherry();
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
