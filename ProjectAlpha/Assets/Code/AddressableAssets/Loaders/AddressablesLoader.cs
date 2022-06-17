using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using Object = UnityEngine.Object;

namespace Code.AddressableAssets;

public class AddressablesLoader : IScopedAddressablesLoader
{
    private readonly Dictionary<Type, object> _typeToHandleStorage;
    private readonly Dictionary<GameObject, GameObject> _instanceToPrefab;
    private bool _isDisposed;

    public ICreator Creator { get; }

    [Inject]
    public AddressablesLoader(ICreator creator) : this(creator, new(), new()) { }

    //so that's what it's for...
    private protected AddressablesLoader(
        ICreator creator,
        Dictionary<Type, object> typeToHandleStorage,
        Dictionary<GameObject, GameObject> instanceToPrefab)
    {
        Creator = creator;
        _typeToHandleStorage = typeToHandleStorage;
        _instanceToPrefab = instanceToPrefab;
    }

    public async UniTask<T> InstantiateAsync<T>(Address<T> address, bool inject = true)
        where T : Object
    {
        if (_isDisposed) return null;

        if (!IsComponent<T>() && !IsGameObject<T>())
        {
            Debug.LogWarning("<T> must be a Component or MonoBehaviour, use LoadAssetAsync<T> instead");
            return await LoadAssetAsync(address);
        }

        GameObject prefab = await LoadAssetTAsync(address.As<GameObject>());

        GameObject instance = Creator.Instantiate(prefab, inject);
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

            if (!prefab.TryGetComponent(out T component))
                Debug.LogError("No component of type " + typeof(T).Name + " found on prefab " + prefab.name);

            return component;
        }

        return await LoadAssetTAsync(address);
    }

    public bool IsLoaded<T>(T instance) where T : Object
    {
        if (_isDisposed || instance == null)
            return false;

        if (TryGetGameObject(instance, out GameObject go) && _instanceToPrefab.ContainsKey(go))
            return true;

        return Storage<T>().ContainsAsset(instance);
    }

    public void Release<T>(T instance) where T : Object
    {
        if (_isDisposed || instance == null)
            return;

        if (!TryGetGameObject(instance, out GameObject go))
        {
            ReleaseT(instance);
            return;
        }

        if (_instanceToPrefab.ContainsKey(go))
        {
            GameObject prefab = _instanceToPrefab.Pop(go);
            ReleaseT(prefab);
            Object.Destroy(instance);
            return;
        }

        // for the LoadAssetAsync<Component/GameObject>() case:
        ReleaseT(instance);

        if (!go.IsPrefab())
            Object.Destroy(instance);
    }

    public void Dispose()
    {
        _isDisposed = true;

        foreach (var loader in _typeToHandleStorage.Values)
            ((IDisposable)loader).Dispose();

        _typeToHandleStorage.Clear();
        _instanceToPrefab.Clear();
    }

    private UniTask<T> LoadAssetTAsync<T>(Address<T> address) where T : Object
    {
        return Storage<T>().AddAssetAsync(address);
    }

    private void ReleaseT<T>(T prefab) where T : Object
    {
        Storage<T>().RemoveAsset(prefab);
    }

    private HandleStorage<T> Storage<T>() where T : Object
    {
        Type type = typeof(T);
        if (!_typeToHandleStorage.TryGetValue(type, out object loader))
        {
            loader = new HandleStorage<T>();
            _typeToHandleStorage[type] = loader;
        }

        return (HandleStorage<T>)loader;
    }

    private static bool TryGetGameObject<T>(T instance, out GameObject gameObject) where T : Object
    {
        switch (instance)
        {
            case GameObject go:
                gameObject = go;
                return true;
            case Component c:
                gameObject = c.gameObject;
                return true;
            default:
                gameObject = null;
                return false;
        }
    }

    private static bool IsComponent<T>() where T : Object =>
        typeof(T).IsAssignableTo(typeof(Component));

    private static bool IsGameObject<T>() where T : Object =>
        typeof(T) == typeof(GameObject);
}