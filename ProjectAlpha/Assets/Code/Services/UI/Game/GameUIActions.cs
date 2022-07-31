using Code.Services.Infrastructure;
using Code.States;
using UnityEngine.iOS;
using VContainer;

namespace Code.Services.UI;

public sealed class GameUIActions : IGameUIActions
{
    [Inject] private GameUIController _gameUIController;
    [Inject] private IGameSceneNavigator _gameSceneNavigator;
    [Inject] private IGameStateMachine _gameStateMachine;

    public void RequestStoreReview() => Device.RequestStoreReview();
    public void HideGameOver() => _gameUIController.HideGameOver();
    public void LoadMenu() => _gameSceneNavigator.NavigateToMenu();
    public void Restart() => _gameStateMachine.Enter<RestartState>(); //black fade out
}
