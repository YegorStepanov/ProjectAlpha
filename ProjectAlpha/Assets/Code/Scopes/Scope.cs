using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Code.AddressableAssets;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine.Assertions;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using VContainer.Unity;
using Debug = UnityEngine.Debug;

namespace Code.Scopes;

public abstract class Scope : LifetimeScope
{
    private AddressablesLoader _loader;
    private CancellationToken _token;
    private bool _isBuilt;
    private UnityEngine.SceneManagement.Scene _scene;

    [UsedImplicitly]
    private new async UniTask Awake()
    {
        _token = this.GetCancellationTokenOnDestroy();
        AssertAutoRunIsDisabled();
        await InitRoot();
        SetScopeParent();
        await BuildAsync();
    }

    public async UniTask BuildAsync(SceneInstance scene)
    {
        //VContainer issue, it spawns new gameobjects in the active scene
        _scene = scene.Scene;

        if (_token == default) //todo: remove
            Debug.Log("Token is default", this);

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

    private void SetScopeParent()
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

    private async Task PreloadScopeAsync()
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

    protected abstract UniTask PreloadAsync(IAddressablesLoader loader);

    [Conditional("DEVELOPMENT")]
    private void AssertAutoRunIsDisabled()
    {
        bool autoRun = GetPrivateInstanceField(typeof(LifetimeScope), this, "autoRun");
        Assert.IsFalse(autoRun);

        static bool GetPrivateInstanceField(Type type, object instance, string fieldName)
        {
            FieldInfo field = type.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            return (bool)field!.GetValue(instance);
        }
    }
}