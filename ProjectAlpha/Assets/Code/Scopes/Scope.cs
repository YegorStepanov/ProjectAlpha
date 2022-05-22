using System;
using System.Reflection;
using Code.AddressableAssets;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine.Assertions;
using VContainer.Unity;

namespace Code.Scopes;

public abstract class Scope : LifetimeScope
{
    private AddressablesLoader _loader;

    [UsedImplicitly]
    private new async UniTask Awake()
    {
        AssertAutoRunIsDisabled();

        if (VContainerSettings.Instance.RootLifetimeScope is Scope { Container: null } rootScope)
        {
            await rootScope.PreloadAndBuildAsync();
        }

        base.Awake();
        await PreloadAndBuildAsync();
    }

    private async UniTask PreloadAndBuildAsync()
    {
        _loader = new AddressablesLoader(this);
        await PreloadAsync(_loader);
        Build();
    }

    protected override void OnDestroy()
    {
        _loader.Dispose();
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