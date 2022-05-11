using System.Threading;
using Code.Services;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace Code.Scopes;

public sealed class BootstrapStart : IAsyncStartable
{
    private readonly LoadingScreen _loadingScreen;
    private readonly SceneLoader _sceneLoader;

    public BootstrapStart(SceneLoader sceneLoader, LoadingScreen loadingScreen)
    {
        _sceneLoader = sceneLoader;
        _loadingScreen = loadingScreen;
    }

    public async UniTask StartAsync(CancellationToken token)
    {
        _loadingScreen.Show();

        await _sceneLoader.LoadAsync<GameScene>(null, token);
        await _sceneLoader.LoadAsync<MenuScene>(null, token);

        await _loadingScreen.HideAsync();
        await _sceneLoader.UnloadAsync<BootstrapScene>(token);
    }
}