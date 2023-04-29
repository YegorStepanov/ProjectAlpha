using Code.Services.Infrastructure;
using Code.UI;
using Cysharp.Threading.Tasks;

namespace Code.Services.Navigators
{
    public sealed class MenuSceneNavigator : IMenuSceneNavigator
    {
        private readonly ISceneLoader _sceneLoader;
        private readonly ILoadingScreen _loadingScreen;
        private readonly GameResourcesLoadedEventAwaiter _gameResourcesLoadedEvent;

        public MenuSceneNavigator(ISceneLoader sceneLoader, ILoadingScreen loadingScreen, GameResourcesLoadedEventAwaiter gameResourcesLoadedEvent)
        {
            _sceneLoader = sceneLoader;
            _loadingScreen = loadingScreen;
            _gameResourcesLoadedEvent = gameResourcesLoadedEvent;
        }

        public void RestartMenuScene()
        {
            Impl().Forget();

            async UniTaskVoid Impl()
            {
                await _loadingScreen.FadeInAsync();

                await _sceneLoader.UnloadAsync<GameScene>();
                await _sceneLoader.LoadAsync<GameScene>();

                await _sceneLoader.UnloadAsync<MenuScene>();
                await _sceneLoader.LoadAsync<MenuScene>();

                await _gameResourcesLoadedEvent.Wait();
                await _loadingScreen.FadeOutAsync();
            }
        }
    }
}