using Code.Services.Infrastructure;
using Code.UI;
using Cysharp.Threading.Tasks;

namespace Code.Services.Navigators
{
    public sealed class BootstrapSceneNavigator : IBootstrapSceneNavigator
    {
        private readonly ISceneLoader _sceneLoader;
        private readonly ILoadingScreen _loadingScreen;
        private readonly GameResourcesLoadedEventAwaiter _gameResourcesLoadedEvent;

        public BootstrapSceneNavigator(ISceneLoader sceneLoader, ILoadingScreen loadingScreen, GameResourcesLoadedEventAwaiter gameResourcesLoadedEvent)
        {
            _sceneLoader = sceneLoader;
            _loadingScreen = loadingScreen;
            _gameResourcesLoadedEvent = gameResourcesLoadedEvent;
        }

        public void NavigateToMenuAndGameScenes()
        {
            Impl().Forget();

            async UniTaskVoid Impl()
            {
                await _loadingScreen.FadeInAsync();

                _ = _sceneLoader.LoadAsync<MenuScene>();
                _ = _sceneLoader.LoadAsync<GameScene>();
                await _gameResourcesLoadedEvent.Wait();

                await _loadingScreen.FadeOutAsync();
                await _sceneLoader.UnloadAsync<BootstrapScene>();
            }
        }
    }
}