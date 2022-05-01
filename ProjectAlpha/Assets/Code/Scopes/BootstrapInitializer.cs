using System.Threading;
using Code.Services;
using Cysharp.Threading.Tasks;
using Zenject;

namespace Code.Scopes;

public sealed class BootstrapInitializer : IInitializable
{
    private readonly LoadingScreen _loadingScreen;
    private readonly SceneLoader _sceneLoader;
    private readonly CancellationToken _token;

    public BootstrapInitializer(SceneLoader sceneLoader, LoadingScreen loadingScreen, CancellationToken token)
    {
        _sceneLoader = sceneLoader;
        _loadingScreen = loadingScreen;
        _token = token;
    }

    public void Initialize() =>
        InitializeAsync().Forget();

    private async UniTaskVoid InitializeAsync()
    {
        _loadingScreen.Show();

        UniTask menuLoading = _sceneLoader.LoadAsync<MenuScene>(_token);
        UniTask gameLoading = _sceneLoader.LoadAsync<GameScene>(_token);

        await (menuLoading, gameLoading);

        await _loadingScreen.HideAsync();
        await _sceneLoader.UnloadAsync<BootstrapScene>(_token);
    }
}