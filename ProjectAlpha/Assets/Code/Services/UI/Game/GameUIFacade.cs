using Code.Services.Navigators;
using UnityEngine.iOS;
using VContainer;

namespace Code.Services.UI;

public sealed class GameUIFacade : IGameUIFacade
{
    [Inject] private GameUIController _gameUIController;
    [Inject] private IGameSceneNavigator _sceneNavigator;

    public void RequestStoreReview() => Device.RequestStoreReview();
    public void LoadMenu() => _sceneNavigator.NavigateToMenuScene();
    public void Restart() => _sceneNavigator.RestartGameScene();
}
