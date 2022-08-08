using Code.Services.Infrastructure;
using Code.UI;
using Cysharp.Threading.Tasks;

namespace Code.Services.Navigators;

public sealed class BootstrapSceneNavigator : IBootstrapSceneNavigator
{
    private readonly ISceneLoader _sceneLoader;
    private readonly ILoadingScreen _loadingScreen;

    public BootstrapSceneNavigator(ISceneLoader sceneLoader, ILoadingScreen loadingScreen)
    {
        _sceneLoader = sceneLoader;
        _loadingScreen = loadingScreen;
    }

    public void NavigateToMenuAndGameScenes()
    {
        Impl().Forget();

        async UniTaskVoid Impl()
        {
            await _loadingScreen.FadeInAsync();

            await _sceneLoader.LoadAsync<MenuScene>();
            await _sceneLoader.LoadAsync<GameScene>();

            await _loadingScreen.FadeOutAsync();
            await _sceneLoader.UnloadAsync<BootstrapScene>();
        }
    }
}
