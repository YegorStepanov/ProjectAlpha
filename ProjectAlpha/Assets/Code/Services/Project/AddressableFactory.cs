using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using VContainer;
using VContainer.Unity;

namespace Code.Services;

public class AddressableFactory : IDisposable
{
    //rename
    private readonly Dictionary<object, object> _assetToHandle = new();

    private readonly Transform _scopeTransform;
    private readonly IObjectResolver _resolver;

    public AddressableFactory(LifetimeScope scope, IObjectResolver resolver)
    {
        _scopeTransform = scope.transform;
        _resolver = resolver;
        
        Debug.Log("Add Factory, scope parent: " + scope.transform.name);
        // todo: Addressables.InitializeAsync() somewhere
    }

    public void Dispose()
    {
        //replace to .Values
        foreach (object handle in _assetToHandle.Values)
            Addressables.Release(handle);

        _assetToHandle.Clear();
    }

    public async UniTask<GameObject> InstantiateAsync(Address address)
    {
        GameObject go = await Addressables.InstantiateAsync(address.Key, _scopeTransform);
        
        if(go == null)
            Debug.Log("Wrong key: " + address.Key);
        
        go.transform.SetParent(null);
        go.name = address.Key;

        _resolver.InjectGameObject(go);
        return go; //.GetComponent()
    }
    
    //todo: add LoadAssetExplicitAsync?
    //it should works with GameObjects(by InstantiateAsync)?
    public async UniTask<T> LoadAssetAsync<T>(Address address) where T : class
    {
        //mb exists already
        AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(address.Key);
        T asset = await handle;
        _assetToHandle.Add(asset, handle);
        return asset;
    }

    [SuppressMessage("ReSharper", "MemberCanBeMadeStatic.Global")]
    public void ReleaseInstance(GameObject go) =>
        Addressables.ReleaseInstance(go);

    public void ReleaseAsset<T>(T asset) where T : class
    {
        if (asset == null) return;

        var handle = (AsyncOperationHandle<T>)_assetToHandle[asset]; //trygetvalue
        _assetToHandle.Remove(asset);
        Addressables.Release(handle);
    }
}