using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using VContainer;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace Code.Services;

public sealed class AddressableFactory : IDisposable
{
    private readonly Dictionary<Object, AsyncOperationHandle> _assetToHandle = new();
    private readonly Dictionary<GameObject, GameObject> _instanceToPrefab = new();

    private readonly LifetimeScope _scope;
    private readonly IObjectResolver _resolver;

    public AddressableFactory(LifetimeScope scope, IObjectResolver resolver)
    {
        _scope = scope;
        _resolver = resolver;
        // Addressables.InitializeAsync() somewhere
    }

    public void Dispose()
    {
        foreach ((Object asset, AsyncOperationHandle handle) in _assetToHandle)
            ReleaseUntyped(asset, handle);

        _assetToHandle.Clear();
    }

    public async UniTask<T> LoadAsync<T>(Address<T> address) where T : Object
    {
        AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(address.Key);
        T asset = await handle;
        _assetToHandle[asset] = handle;

        if (asset is GameObject prefab)
            return LoadGameObjectAsync(address, prefab);

        if (asset is MonoBehaviour mb)
            _resolver.Inject(mb);

        return asset;
    }

    public void Release<T>(T instance) where T : Object
    {
        if (instance is GameObject go)
        {
            ReleaseGameObject(go);
            return;
        }

        AsyncOperationHandle handle = _assetToHandle.Pop(instance);
        AsyncOperationHandle<T> handleT = handle.Convert<T>();
 
        Addressables.Release(handleT);
    }

    private T LoadGameObjectAsync<T>(Address<T> address, GameObject prefab) where T : Object
    {
        GameObject instance = _scope.InstantiateInScene(prefab);

        instance.name = address.Key;
        _instanceToPrefab[instance] = prefab;
        return instance as T;
    }

    private void ReleaseGameObject(GameObject instance)
    {
        GameObject prefab = _instanceToPrefab.Pop(instance);
        AsyncOperationHandle handle = _assetToHandle.Pop(prefab);

        AsyncOperationHandle<GameObject> handleT = handle.Convert<GameObject>();

        Addressables.Release(handleT);
        Object.Destroy(instance);
    }

    // warn: `Addressables.Release()` does not work with untyped AsyncOperationHandle, it requires AsyncOperationHandle<T>
    private static void ReleaseUntyped(Object asset, AsyncOperationHandle handle)
    {
        switch (asset)
        {
            case GameObject:
                Addressables.Release(handle.Convert<GameObject>());
                break;
            case Sprite:
                Addressables.Release(handle.Convert<Sprite>());
                break;
            default:
                Debug.LogError("Unhandled disposing of type: " + asset.GetType().Name);
                break;
        }
    }
}