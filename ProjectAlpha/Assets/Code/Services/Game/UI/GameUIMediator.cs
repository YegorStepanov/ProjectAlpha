using Code.States;
using UnityEngine.iOS;
using VContainer;

namespace Code.Services.Game.UI;

public sealed class GameUIMediator : IGameUIMediator
{
    [Inject] private GameUIController _gameUIController;
    [Inject] private GameSceneLoader _gameSceneLoader;
    [Inject] private IGameStateMachine _gameStateMachine;

    public void RequestStoreReview() => Device.RequestStoreReview();
    public void HideGameOver() => _gameUIController.HideGameOver();
    public void LoadMenu() => _gameSceneLoader.LoadMenu();
    public void Restart() => _gameStateMachine.Enter<RestartState>(); //black fade out
}
