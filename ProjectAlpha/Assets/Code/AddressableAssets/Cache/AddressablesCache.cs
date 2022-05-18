using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Object = UnityEngine.Object;

namespace Code.AddressableAssets;

public class AddressablesCache : IDisposable, IAddressablesCache
{
    private readonly Dictionary<Type, IAddressableAssetCache<Object>> _caches = new();
    private bool _isDisposed;

    public UniTask<int> CacheAssetAsync<T>(Address<T> address) where T : Object
    {
        if (_isDisposed) return UniTask.FromResult(0);
        var cache = GetOrCreateCache<T>();
        return cache.CacheAssetAsync(address.Key);
    }

    public int ReleaseCachedAsset<T>(Address<T> address) where T : Object
    {
        if (_isDisposed) return 0;
        var cache = GetOrCreateCache<T>();
        return cache.ReleaseCachedAsset(address.Key);
    }

    public void RemoveCachedAsset<T>(Address<T> address) where T : Object
    {
        if (_isDisposed) return;
        var cache = GetOrCreateCache<T>();
        cache.RemoveCachedAsset(address.Key);
    }

    private IAddressableAssetCache<T> GetOrCreateCache<T>() where T : Object
    {
        Type type = typeof(T);
        if (!_caches.TryGetValue(type, out IAddressableAssetCache<Object> cache))
        {
            cache = new AddressableAssetCache<T>();
            _caches[type] = cache;
        }

        return (IAddressableAssetCache<T>)cache;
    }
    
    public void Dispose()
    {
        _isDisposed = true;
        foreach (var cache in _caches.Values)
            cache.Dispose();
        _caches.Clear();
    }
}