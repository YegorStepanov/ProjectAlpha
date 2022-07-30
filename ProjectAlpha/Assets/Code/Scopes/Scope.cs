using System;
using System.Reflection;
using System.Threading;
using Code.AddressableAssets;
using Code.AddressableAssets.Loaders;
using Code.Common;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine.Assertions;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;
using Scene = UnityEngine.SceneManagement.Scene;

namespace Code.Scopes;

public abstract class Scope : LifetimeScope
{
    private AddressablesLoader _loader;
    private CancellationToken _token;
    private bool _isBuilt;
    private Scene _scene;

    [UsedImplicitly]
    private new async UniTask Awake()
    {
        _token = this.GetCancellationTokenOnDestroy();
        AssertAutoRunIsDisabled();
        await InitRoot();
        SetParentScope();
        await BuildAsync();
    }

    public async UniTask WaitForBuild(SceneInstance scene)
    {
        //VContainer issue, it spawns new gameobjects in the active scene
        _scene = scene.Scene;

        while (!_isBuilt)
            await UniTask.Yield(_token);
    }

    private static UniTask InitRoot()
    {
        if (VContainerSettings.Instance.RootLifetimeScope is Scope rootScope)
        {
            bool isRootCreated = rootScope.Container != null;
            if (!isRootCreated)
                return rootScope.BuildAsync();
        }

        return UniTask.CompletedTask;
    }

    private void SetParentScope()
    {
        base.Awake();
    }

    private async UniTask BuildAsync()
    {
        await PreloadScopeAsync();
        SetActiveScene();
        Build();
        _isBuilt = true;
    }

    private async UniTask PreloadScopeAsync()
    {
        _loader = new AddressablesLoader(new Creator(this));
        await PreloadAsync(_loader);
    }

    private void SetActiveScene()
    {
        if (_scene.IsValid())
            SceneManager.SetActiveScene(_scene);
    }

    protected override void OnDestroy()
    {
        _loader?.Dispose();
        base.OnDestroy();
    }

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterInstance(_token);
        ConfigureServices(builder);
    }

    protected abstract void ConfigureServices(IContainerBuilder builder);

    protected abstract UniTask PreloadAsync(IAddressablesLoader loader);

    private void AssertAutoRunIsDisabled()
    {
        if (!PlatformInfo.IsDevelopment) return;

        bool autoRun = GetPrivateInstanceField(typeof(LifetimeScope), this, "autoRun");
        Assert.IsFalse(autoRun);

        static bool GetPrivateInstanceField(Type type, object instance, string fieldName)
        {
            FieldInfo field = type.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            return (bool)field!.GetValue(instance);
        }
    }
}
