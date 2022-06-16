using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Object = UnityEngine.Object;

namespace Code.AddressableAssets;

public class AddressablesCache : IAddressablesCache, IDisposable
{
    private readonly Dictionary<Type, object> _typeToHandleStorage = new();
    private bool _isDisposed;

    public UniTask CacheAssetAsync<T>(Address<T> address) where T : Object
    {
        if (_isDisposed) return UniTask.FromResult(0);
        var storage = GetStorage<T>();
        return storage.AddAssetAsync(address);
    }

    public void RemoveCachedAsset<T>(Address<T> address) where T : Object
    {
        if (_isDisposed) return;
        var storage = GetStorage<T>();
        storage.RemoveAsset(address);
    }

    public void RemoveAllCachedAssets<T>(Address<T> address) where T : Object
    {
        if (_isDisposed) return;
        var storage = GetStorage<T>();
        // if (storage.CountAssets(address) > 0)
        storage.RemoveAllAssets(address);
    }

    private HandleStorage<T> GetStorage<T>() where T : Object
    {
        Type type = typeof(T);
        if (!_typeToHandleStorage.TryGetValue(type, out object storage))
        {
            storage = new HandleStorage<Object>();
            _typeToHandleStorage[type] = storage;
        }

        return (HandleStorage<T>)storage;
    }

    public void Dispose()
    {
        _isDisposed = true;

        foreach (object storage in _typeToHandleStorage.Values)
            ((IDisposable)storage).Dispose();
        _typeToHandleStorage.Clear();
    }
}