using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace Code.AddressableAssets;

internal sealed class AddressableAssetCache<TAsset> : IAddressableAssetCache<TAsset> where TAsset : Object
{
    // Addressables.LoadAssetAsync() returns the same handle usually, but sometimes returns a different one (differs by m_Version)...
    // and the dictionary has big overhead here 
    private readonly List<(AsyncOperationHandle<TAsset> Handle, string Address)> _handleToAddress = new();

    private bool _isDisposed;

    public async UniTask<int> CacheAssetAsync(string address)
    {
        if (_isDisposed)
            return 0;

        AsyncOperationHandle<TAsset> handle = Addressables.LoadAssetAsync<TAsset>(address);

        _handleToAddress.Add((handle, address));

        int count = CountAddresses(address);

        try
        {
            await handle;
            return count;
        }
        catch
        {
            Addressables.Release(handle);
            _handleToAddress.Remove((handle, address));
            throw;
        }
    }

    public int ReleaseCachedAsset(string address)
    {
        int count = CountAddresses(address);
        if (count == 0)
            return -1;

        AsyncOperationHandle<TAsset> handle = FindHandle(address);

        Addressables.Release(handle);
        _handleToAddress.Remove((handle, address));

        return count - 1;
    }

    public void RemoveCachedAsset(string address)
    {
        for (int i = _handleToAddress.Count - 1; i >= 0; i--)
        {
            var pair = _handleToAddress[i];
            if (pair.Address != address) continue;

            Addressables.Release(pair.Handle);
            _handleToAddress.Remove(pair);
        }
    }

    private AsyncOperationHandle<TAsset> FindHandle(string address)
    {
        foreach (var pair in _handleToAddress)
        {
            if (pair.Address == address)
                return pair.Handle;
        }

        throw new Exception($"Handle not found for address: {address}");
    }

    private int CountAddresses(string address)
    {
        int count = 0;

        foreach (var pair in _handleToAddress)
        {
            if (pair.Address == address)
                count++;
        }

        return count;
    }

    public void Dispose()
    {
        foreach (var pair in _handleToAddress)
            Addressables.Release(pair.Handle);

        _handleToAddress.Clear();
    }
}