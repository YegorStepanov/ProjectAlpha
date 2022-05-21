using System;
using Code.AddressableAssets;
using Code.Game;
using Code.Services;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Code.Scopes;

public static class ProjectAddress
{
    public static readonly Address<GameObject> Graphy = new("Graphy");
    public static readonly Address<CameraController> Camera = new("Camera"); //gameobject?
    public static readonly Address<GameSettings> Settings = new("Settings");
    
    public static readonly Address<LoadingScreen> LoadingScreen = new("LoadingScreen");
}

public sealed class ProjectScope : LifetimeScope
{
    GameObject graphy
    CameraController camera
    GameSettings settings
    LoadingScreen loadingScreen
    
    protected override void Awake()
    {
        GameObject graphy = await loader.InstantiateAsync(ProjectAddress.Graphy);
        CameraController camera = await loader.InstantiateAsync(ProjectAddress.Camera);
        GameSettings settings = await loader.LoadAssetAsync(ProjectAddress.Settings);
        LoadingScreen loadingScreen = await loader.InstantiateAsync(ProjectAddress.LoadingScreen);
        
        base.Awake();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    //get preconfigure or configureAsync or ??
    public static async UniTask<Action<IContainerBuilder>> Preload(IGlobalAddressablesLoader loader)
    {
        

        return builder =>
        {
            builder.RegisterInstance(graphy); //without inject
            
            // builder.RegisterComponentInNewPrefab(cameraPrefab, Lifetime.Singleton);
            // builder.RegisterComponent(settings); //with inject
            settings.RegisterAllSettings(builder);
            builder.RegisterInstance(new ProjectScopeUnloader(loader, graphy, camera, settings));
            
            builder.RegisterInstance(loadingScreen);
        };
    }

    protected override void Configure(IContainerBuilder builder)
    {
        //RegisterComponentInNewPrefab
        //vs
        //RegisterComponentOnNewGameObject

        // public LoadingScreen _loadingScreen;
        // builder.RegisterComponent(_loadingScreen);
        
        builder.RegisterEntryPoint<ProjectStart>();
    }

    // protected override void Configure(IContainerBuilder builder)
    // {
    //     Instantiate(_graphy);
    //
    //
    //     _gameSettings.RegisterAllSettings(builder);
    //
    //     builder.RegisterComponentInNewPrefab(_cameraController, Lifetime.Singleton); //GOname? nonlazy?
    // }
}

public sealed class ProjectScopeUnloader : IDisposable
{
    private readonly IGlobalAddressablesLoader _loader;
    private readonly GameObject _graphy;
    private readonly CameraController _camera;
    private readonly GameSettings _settings;

    public ProjectScopeUnloader(
        IGlobalAddressablesLoader loader, 
        GameObject graphy, 
        CameraController camera,
        GameSettings settings)
    {
        _loader = loader;
        _graphy = graphy;
        _camera = camera;
        _settings = settings;
    }

    public void Dispose()
    {
        Debug.Log("UNLOADER DISPOSE");
            
        _loader.Release(_graphy);
        _loader.Release(_camera);
        _loader.Release(_settings);
    }
}