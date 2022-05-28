using System.Threading;
using Code.Services;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace Code.Scopes;

public sealed class BootstrapStart : IAsyncStartable
{
    private readonly ISceneLoader _sceneLoader;
    private readonly LoadingScreen _loadingScreen;

    public BootstrapStart(ISceneLoader sceneLoader, LoadingScreen loadingScreen)
    {
        _sceneLoader = sceneLoader;
        _loadingScreen = loadingScreen;
    }

    public async UniTask StartAsync(CancellationToken token)
    {
        _loadingScreen.Show();

        UniTask gameLoad = _sceneLoader.LoadAsync<GameScene>(token);
        UniTask menuLoad = _sceneLoader.LoadAsync<MenuScene>(token);

        await (loadGame: gameLoad, loadMenu: menuLoad);

        await _loadingScreen.FadeOutAsync();
        await _sceneLoader.UnloadAsync<BootstrapScene>(token);
    }
}