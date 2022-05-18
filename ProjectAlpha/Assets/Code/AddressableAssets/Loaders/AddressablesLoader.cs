using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace Code.AddressableAssets;

public class AddressablesLoader : IScopedAddressablesLoader
{
    private readonly LifetimeScope _scope;

    private readonly Dictionary<Type, IAddressableAssetLoader<Object>> _loaders;
    private readonly Dictionary<GameObject, GameObject> _instanceToPrefab;

    private bool _isDisposed;

    [Inject]
    internal AddressablesLoader(LifetimeScope scope) :
        this(scope, new(), new()) { }

    //so that's what it's for...
    private protected AddressablesLoader(
        LifetimeScope scope,
        Dictionary<Type, IAddressableAssetLoader<Object>> loaders,
        Dictionary<GameObject, GameObject> instanceToPrefab)
    {
        _scope = scope;
        _loaders = loaders;
        _instanceToPrefab = instanceToPrefab;
    }

    public async UniTask<T> InstantiateAsync<T>(Address<T> address, Transform under = null) where T : Component
    {
        if (_isDisposed) return null;
        GameObject instance = await InstantiateTAsync(address, under);
        return instance.GetComponent<T>();
    }
    
    public UniTask<GameObject> InstantiateAsync(Address<GameObject> address, Transform under = null)
    {
        if (_isDisposed) return UniTask.FromResult<GameObject>(null);
        return InstantiateTAsync(address, under);
    }

    private async UniTask<GameObject> InstantiateTAsync<T>(Address<T> address, Transform under) where T : Object
    {
        var loader = GetOrCreateLoader<GameObject>();

        var prefab = await loader.LoadAssetAsync(address.Key) as GameObject; //
        GameObject instance = _scope.Instantiate(prefab, address, under);
        _instanceToPrefab[instance] = prefab;
        return instance;
    }

    public async UniTask<T> LoadAssetAsync<T>(Address<T> address) where T : Object
    {
        if (typeof(T).IsAssignableTo(typeof(Component)))
            throw new ArgumentException("<T> must not be a Component/MonoBehaviour");
        
        if (_isDisposed) return null;
        
        var loader = GetOrCreateLoader<T>();
        Object asset = await loader.LoadAssetAsync(address.Key);
        return asset as T;
    }

    public bool IsLoaded<T>(T instance) where T : Object
    {
        if (instance is not GameObject and not MonoBehaviour)
        {
            IAddressableAssetLoader<T> assetLoader = GetOrCreateLoader<T>();
            return assetLoader.IsLoaded(instance);
        }

        GameObject go = instance is MonoBehaviour mb 
            ? mb.gameObject 
            : instance as GameObject;

        bool isCachedInstance = _instanceToPrefab.ContainsKey(go);
        if (isCachedInstance)
            return true;
        
        IAddressableAssetLoader<T> loader = GetOrCreateLoader<T>();
        return loader.IsLoaded(instance);
    }

    public void Release<T>(T instance) where T : Object
    {
        //check for null!
        if (_isDisposed) return;

        if (instance == null) return;
        
        if (instance is not GameObject and not MonoBehaviour)
        {            
            T asset = instance;
            ReleaseT<T>(asset);
            return;
        }
        
        GameObject go = instance is MonoBehaviour mb 
            ? mb.gameObject 
            : instance as GameObject;
        
        if (_instanceToPrefab.ContainsKey(go))
        {
            GameObject prefab = _instanceToPrefab.Pop(go);
            ReleaseT<GameObject>(prefab);
            Object.Destroy(instance);
            return;
        }
        
        ReleaseT<T>(instance);
       
        if(!go.IsPrefab())
           Object.Destroy(instance);
    }

    private void ReleaseT<T>(Object prefab) where T : Object
    {
        var loader = GetOrCreateLoader<T>();
        loader.Release(prefab);
    }

    private IAddressableAssetLoader<T> GetOrCreateLoader<T>() where T : Object
    {
        Type type = typeof(T);
        if (!_loaders.TryGetValue(type, out IAddressableAssetLoader<Object> loader))
        {
            loader = new AddressableAssetLoader<T>();
            _loaders[type] = loader;
        }

        return (IAddressableAssetLoader<T>)loader;
    }

    public void Dispose()
    {
        _isDisposed = true;
        foreach (var loader in _loaders.Values)
            loader.Dispose();
        _loaders.Clear();

        _instanceToPrefab.Clear();
    }
}