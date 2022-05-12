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
    protected record struct HandleData(AsyncOperationHandle Handle, int Count);

    private readonly Dictionary<Object, HandleData> _assetToHandle;
    private readonly Dictionary<GameObject, GameObject> _instanceToPrefab;
    private readonly Dictionary<AddressData, AsyncOperationHandle> _cachedAssetToHandle;

    private readonly LifetimeScope _scope;

    private bool _isDisposed;

    [Inject, UsedImplicitly]
    public ScopedAddressableFactory(LifetimeScope scope, GlobalAddressableFactory globalFactory) :
        this(scope, new(), new(), new()) { }

    protected ScopedAddressableFactory(
        LifetimeScope scope,
        Dictionary<Object, HandleData> assetsToHandle,
        Dictionary<GameObject, GameObject> instanceToPrefab,
        Dictionary<AddressData, AsyncOperationHandle> cachedAssetToHandle)
    {
        _scope = scope;
        _assetToHandle = assetsToHandle;
        _instanceToPrefab = instanceToPrefab;
        _cachedAssetToHandle = cachedAssetToHandle;
    }

    public virtual void Dispose()
    {
        _isDisposed = true;

        foreach ((Object asset, HandleData data) in _assetToHandle)
        {
            Type type = asset.GetType();
            for (int i = 0; i < data.Count; i++)
                ReleaseUntyped(type, data.Handle);
        }

        foreach ((AddressData data, AsyncOperationHandle handle) in _cachedAssetToHandle)
            ReleaseUntyped(data.Type, handle);

        _assetToHandle.Clear();
        _instanceToPrefab.Clear();
        _cachedAssetToHandle.Clear();
    }

    public async UniTask PreloadAsync<T>(Address<T> address) where T : Object
    {
        // await Addressables.DownloadDependenciesAsync(address.Key, true);

        if (_isDisposed)
            return;

        AddressData data = address.AsData();
        if (_cachedAssetToHandle.ContainsKey(data))
            return;

        AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(address.Key);
        _cachedAssetToHandle[data] = handle;
        await handle;
    }

    public async UniTask<T> LoadAsync<T>(Address<T> address) where T : Object
    {
        if (_isDisposed)
            return default;

        if (typeof(T) == typeof(GameObject))
        {
            GameObject prefab = await LoadAssetAsync(address.As<GameObject>());
            GameObject instance = _scope.InstantiateInScene(prefab, address);
            _instanceToPrefab[instance] = prefab;

            return instance as T;
        }

        if (IsComponent())
        {
            GameObject prefab = await LoadAssetAsync(address.As<GameObject>());
            GameObject instance = _scope.InstantiateInScene(prefab, address);
            _instanceToPrefab[instance] = prefab;

            return instance.GetComponent<T>();
        }

        //Sprite, Texture2D, etc
        return await LoadAssetAsync(address);

        static bool IsComponent() =>
            typeof(Component).IsAssignableFrom(typeof(T));
    }

    private async UniTask<T> LoadAssetAsync<T>(Address<T> address) where T : Object
    {
        AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(address.Key);
        T asset = await handle;

        PushHandle(asset, handle);

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

        AsyncOperationHandle<T> handle = PopHandle(instance);
        Addressables.Release(handle);
    }

    private void ReleaseGameObject(GameObject instance)
    {
        if (!_instanceToPrefab.ContainsKey(instance))
        {
            Debug.LogError("Attempt to release an uncontrolled instance: " + instance.name);
            return;
        }

        GameObject prefab = _instanceToPrefab.Pop(instance);

        AsyncOperationHandle<GameObject> handle = PopHandle(prefab);
        Addressables.Release(handle);
        Object.Destroy(instance);
    }

    private void PushHandle<T>(T asset, AsyncOperationHandle<T> handle) where T : Object
    {
        if (_assetToHandle.TryGetValue(asset, out HandleData data))
            _assetToHandle[asset] = new HandleData(data.Handle, data.Count + 1);
        else
            _assetToHandle[asset] = new HandleData(handle, 1);
    }

    private AsyncOperationHandle<T> PopHandle<T>(T asset) where T : Object
    {
        HandleData handleData = _assetToHandle.Pop(asset);

        if (handleData.Count > 1)
            _assetToHandle[asset] = new HandleData(handleData.Handle, handleData.Count - 1);

        return handleData.Handle.Convert<T>();
    }
    
    // `Addressables.Release()` does not work with untyped AsyncOperationHandle, it requires AsyncOperationHandle<T>
    // generic case can only be resolved by reflection
    private static void ReleaseUntyped(Type type, AsyncOperationHandle handle)
    {
        if (type == typeof(GameObject)) Release(handle.Convert<GameObject>());
        else if (type == typeof(Sprite)) Release(handle.Convert<Sprite>());
        else if (type == typeof(Texture2D)) Release(handle.Convert<Texture2D>());
        else if (type == typeof(Texture3D)) Release(handle.Convert<Texture3D>());
        else if (type == typeof(Texture)) Release(handle.Convert<Texture>());
        else
            Debug.LogError("Unhandled disposing of type: " + type.Name);

        void Release<T>(AsyncOperationHandle<T> typedHandle) =>
            Addressables.Release(typedHandle);
    }
}