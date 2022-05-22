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

    public async UniTask<T> InstantiateAsync<T>(Address<T> address, Transform under = null) where T : Object
    {
        if (IsAsset<T>())
            throw new ArgumentException("<T> must be a Component or MonoBehaviour");

        if (_isDisposed) return null;
        
        GameObject prefab = await LoadAssetTAsync(address.As<GameObject>());

        GameObject instance = _scope.InstantiateInScene(prefab, address, under);
        _instanceToPrefab[instance] = prefab;
        
        if (IsComponent<T>())
            return instance.GetComponent<T>();

        return instance as T;
    }

    public async UniTask<T> LoadAssetAsync<T>(Address<T> address) where T : Object
    {
        if (_isDisposed) return null;

        if (IsComponent<T>())
        {
            GameObject prefab = await LoadAssetTAsync(address.As<GameObject>());
            return prefab.GetComponent<T>();
        }

        return await LoadAssetTAsync(address);
    }

    private async UniTask<T> LoadAssetTAsync<T>(Address<T> address) where T : Object
    {
        return await Loader<T>().LoadAssetAsync(address.Key) as T;
    }

    public bool IsLoaded<T>(T instance) where T : Object
    {
        if (_isDisposed) return false;
        if (instance == null) return false;

        if (IsAsset<T>())
        {
            return Loader<T>().IsLoaded(instance);
        }

        GameObject go = instance is Behaviour mb
            ? mb.gameObject
            : (instance as GameObject)!;

        bool isCachedInstance = _instanceToPrefab.ContainsKey(go);
        if (isCachedInstance)
            return true;

        return Loader<T>().IsLoaded(instance);
    }

    public void Release<T>(T instance) where T : Object
    {
        if (_isDisposed) return;
        if (instance == null) return;

        if (IsAsset<T>())
        {
            ReleaseT(instance);
            return;
        }

        GameObject go = instance is Component mb
            ? mb.gameObject
            : (instance as GameObject)!;

        bool isCachedInstance = _instanceToPrefab.ContainsKey(go);
        if (isCachedInstance)
        {
            GameObject prefab = _instanceToPrefab.Pop(go);
            ReleaseT(prefab);
            Object.Destroy(instance);
            return;
        }

        // for LoadAssetAsync<Component/GameObject>()
        ReleaseT(instance);

        if (!go.IsPrefab())
            Object.Destroy(instance);
    }

    private void ReleaseT<T>(T prefab) where T : Object
    {
        Loader<T>().Release(prefab);
    }

    private IAddressableAssetLoader<T> Loader<T>() where T : Object
    {
        if (!_loaders.TryGetValue(typeof(T), out IAddressableAssetLoader<Object> loader))
        {
            loader = new AddressableAssetLoader<T>();
            _loaders[typeof(T)] = loader;
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

    private static bool IsComponent<T>() where T : Object =>
        typeof(T).IsAssignableTo(typeof(Component));

    private static bool IsGameObject<T>() where T : Object =>
        typeof(T) == typeof(GameObject);

    private static bool IsAsset<T>() where T : Object =>
        !IsComponent<T>() && !IsGameObject<T>();
}