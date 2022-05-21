using System.Threading;
using Code.AddressableAssets;
using Code.Services;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace Code.Scopes;

public sealed class ProjectStart : IAsyncStartable
{
    private readonly LifetimeScope _scope;
    private readonly ISceneLoader _sceneLoader;
    private readonly LoadingScreen _loadingScreen;
    private readonly GlobalAddressablesLoader _loader;

    public ProjectStart(LifetimeScope scope, ISceneLoader sceneLoader, LoadingScreen loadingScreen,
        GlobalAddressablesLoader loader)
    {
        _scope = scope;
        _sceneLoader = sceneLoader;
        _loadingScreen = loadingScreen;
        _loader = loader;
    }

    public async UniTask StartAsync(CancellationToken token)
    {
        _loadingScreen.Show();

        RootScope projectScopePrefab = null;

        PositionGeneratorData positionGeneratorData = await _loader.LoadAssetAsync(DataAddress.PositionGenerator);
        IPositionGenerator positionGenerator = positionGeneratorData.Create();

        WidthGeneratorData widthGeneratorData = await _loader.LoadAssetAsync(DataAddress.WidthGenerator);
        IWidthGenerator widthGenerator = widthGeneratorData.Create();

        LifetimeScope rootScope = _scope.CreateChildFromPrefab(projectScopePrefab, 
            RootScope.Preload(positionGenerator, widthGenerator));

        await _sceneLoader.LoadAsync<GameScene>(rootScope, token);
        await _sceneLoader.LoadAsync<MenuScene>(rootScope, token);

        await _loadingScreen.HideAsync();
        await _sceneLoader.UnloadAsync<BootstrapScene>(token);
    }
}