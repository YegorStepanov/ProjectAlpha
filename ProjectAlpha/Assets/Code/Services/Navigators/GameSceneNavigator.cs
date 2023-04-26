using Code.Services.Infrastructure;
using Code.States;
using Code.UI;
using Cysharp.Threading.Tasks;

namespace Code.Services.Navigators
{
    public sealed class GameSceneNavigator : IGameSceneNavigator
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly ISceneLoader _sceneLoader;
        private readonly ILoadingScreen _loadingScreen;

        public GameSceneNavigator(
            IGameStateMachine gameStateMachine,
            ISceneLoader sceneLoader,
            ILoadingScreen loadingScreen)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _loadingScreen = loadingScreen;
        }

        public void NavigateToMenuScene()
        {
            Impl().Forget();

            async UniTaskVoid Impl()
            {
                await _loadingScreen.FadeInAsync();

                await _sceneLoader.LoadAsync<MenuScene>();
                _gameStateMachine.Enter<StartState>();

                await _loadingScreen.FadeOutAsync();
            }
        }

        public void RestartGameScene()
        {
            _gameStateMachine.Enter<RestartState>();
        }
    }
}
