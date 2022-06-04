using System;
using System.Reflection;
using Code.AddressableAssets;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using VContainer.Unity;

namespace Code.Scopes;

public abstract class Scope : LifetimeScope
{
    //VContainer issue, it spawns new gameobjects in the active scene
    public UnityEngine.SceneManagement.Scene Scene { get; set; }

    private AddressablesLoader _loader;

    private bool _isPreloaded;

    public UniTask OnPreloadedAsync() =>
        UniTask.WaitWhile(() => _isPreloaded == false);

    [UsedImplicitly]
    private new async UniTask Awake()
    {
        AssertAutoRunIsDisabled();

        if (VContainerSettings.Instance.RootLifetimeScope is Scope { Container: null } rootScope)
        {
            await rootScope.PreloadAndBuildAsync();
        }

        // Set parent
        base.Awake();

        await PreloadAndBuildAsync();
    }

    private async UniTask PreloadAndBuildAsync()
    {
        _loader = new AddressablesLoader(this);

        await PreloadAsync(_loader);

        if (Scene.IsValid())
            SceneManager.SetActiveScene(Scene);

        Build();

        _isPreloaded = true;
    }

    protected override void OnDestroy()
    {
        _loader?.Dispose();
        base.OnDestroy();
    }

    protected abstract UniTask PreloadAsync(IAddressablesLoader loader);

    private void AssertAutoRunIsDisabled()
    {
        bool autoRun = (bool)GetPrivateInstanceField(typeof(LifetimeScope), this, "autoRun");
        Assert.IsFalse(autoRun);
    }

    private static object GetPrivateInstanceField(Type type, object instance, string fieldName)
    {
        FieldInfo field = type.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
        return field!.GetValue(instance);
    }
}