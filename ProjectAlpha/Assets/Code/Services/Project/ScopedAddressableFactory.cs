using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using VContainer;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace Code.Services;

public class ScopedAddressableFactory : IDisposable
{
    private readonly Dictionary<Object, AsyncOperationHandle> _assetToHandle;
    private readonly Dictionary<GameObject, GameObject> _instanceToPrefab;
    private readonly Dictionary<AddressData, Object> _dataToPreloadedAsset;

    private readonly LifetimeScope _scope;

    private bool _isDisposed;

    [Inject, UsedImplicitly]
    public ScopedAddressableFactory(LifetimeScope scope, GlobalAddressableFactory globalFactory) :
        this(scope, new(), new(), new()) { }

    protected ScopedAddressableFactory(
        LifetimeScope scope,
        Dictionary<Object, AsyncOperationHandle> assetsToHandle,
        Dictionary<GameObject, GameObject> instanceToPrefab,
        Dictionary<AddressData, Object> dataToPreloadedAsset)
    {
        _scope = scope;
        _assetToHandle = assetsToHandle;
        _instanceToPrefab = instanceToPrefab;
        this._dataToPreloadedAsset = dataToPreloadedAsset;
    }

    public virtual void Dispose()
    {
        foreach ((Object asset, AsyncOperationHandle handle) in _assetToHandle)
            ReleaseUntyped(asset, handle);

        _assetToHandle.Clear();
        _isDisposed = true;

        // _dataToPreloadedAsset.Clear();
    }

    public async UniTask PreloadAsync<T>(Address<T> address) where T : Object
    {
        AddressData data = new(address.Key, typeof(T));

        if (_dataToPreloadedAsset.ContainsKey(data))
        {
            Debug.Log("Return; cache already exists");
            return;
        }

        Debug.Log("Adding cache");
        T preloaded = await LoadAsync(address);
        Debug.Log("Added cache");
        _dataToPreloadedAsset[data] = preloaded;
    }

    public async UniTask<T> LoadAsync<T>(Address<T> address, bool trackHandle = true) where T : Object
    {
        if (_isDisposed)
            return default;

        if (TryGetPreloaded(address, out T preloaded)) 
            return preloaded;

        if (typeof(T) == typeof(GameObject))
        {
            GameObject prefab = await LoadAssetAsync(address.As<GameObject>(), trackHandle);
            GameObject instance = _scope.InstantiateInScene(prefab, address);
            _instanceToPrefab[instance] = prefab;

            return instance as T;
        }

        if (IsComponent())
        {
            GameObject prefab = await LoadAssetAsync(address.As<GameObject>(), trackHandle);
            GameObject instance = _scope.InstantiateInScene(prefab, address);
            _instanceToPrefab[instance] = prefab;

            return instance.GetComponent<T>();
        }

        //Sprite, Texture2D, etc
        return await LoadAssetAsync(address, trackHandle);

        static bool IsComponent() =>
            typeof(Component).IsAssignableFrom(typeof(T));
    }

    private bool TryGetPreloaded<T>(Address<T> address, out T preloaded) where T : Object
    {
        //mb we should try to take from global factory?
        AddressData data = new(address.Key, typeof(T));
        if (!_dataToPreloadedAsset.ContainsKey(data))
        {
            preloaded = default;
            return false;
        }

        Debug.Log("Pop cache");
        preloaded = _dataToPreloadedAsset.Pop(data) as T;
        return true;
    }

    private async UniTask<T> LoadAssetAsync<T>(Address<T> address, bool trackHandle) where T : Object
    {
        AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(address.Key);
        T asset = await handle;

        if (trackHandle)
            _assetToHandle[asset] = handle;

        return asset;
    }

    public void Release<T>(T instance) where T : Object
    {
        if (_isDisposed)
            return;

        if (instance is GameObject go)
        {
            ReleaseGameObject(go);
            return;
        }

        if (!_assetToHandle.ContainsKey(instance))
            return;

        AsyncOperationHandle handle = _assetToHandle.Pop(instance);
        AsyncOperationHandle<T> handleT = handle.Convert<T>();

        Addressables.Release(handleT);
    }

    private void ReleaseGameObject(GameObject instance)
    {
        if (!_instanceToPrefab.ContainsKey(instance))
        {
            Debug.LogError("Attempt to release an uncontrolled instance: " + instance.name);
            return;
        }

        GameObject prefab = _instanceToPrefab.Pop(instance);
        AsyncOperationHandle handle = _assetToHandle.Pop(prefab);

        AsyncOperationHandle<GameObject> handleT = handle.Convert<GameObject>();

        Addressables.Release(handleT);
        Object.Destroy(instance);
    }

    // `Addressables.Release()` does not work with untyped AsyncOperationHandle, it requires AsyncOperationHandle<T>
    // generic case can only be resolved by reflection
    private static void ReleaseUntyped(Object asset, AsyncOperationHandle handle)
    {
        bool success = asset switch
        {
            GameObject => Release(handle.Convert<GameObject>()),
            Sprite => Release(handle.Convert<Sprite>()),
            Texture2D => Release(handle.Convert<Texture2D>()),
            Texture3D => Release(handle.Convert<Texture3D>()),
            Texture => Release(handle.Convert<Texture>()),
            _ => false
        };

        if (!success)
            Debug.LogError("Unhandled disposing of type: " + asset.GetType().Name);

        bool Release<T1>(AsyncOperationHandle<T1> typedHandle)
        {
            Addressables.Release(typedHandle);
            return true;
        }
    }
}