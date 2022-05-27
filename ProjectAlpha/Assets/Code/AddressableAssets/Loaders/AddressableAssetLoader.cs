using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Code.AddressableAssets;

internal sealed class AddressableAssetLoader<TAsset> : IAddressableAssetLoader<TAsset> where TAsset : Object
{
    // Addressables.LoadAssetAsync() returns the same handle usually, but sometimes returns a different one (differs by m_Version)...
    // and the dictionary has big overhead here 
    private readonly List<(AsyncOperationHandle<TAsset> Handle, TAsset Asset)> _handleToAsset = new();

    private bool _isDisposed;

    public async UniTask<Object> LoadAssetAsync(string address)
    {
        AsyncOperationHandle<TAsset> handle = Addressables.LoadAssetAsync<TAsset>(address);
        TAsset asset = await handle;

        _handleToAsset.Add((handle, asset));
        return asset;
    }

    public bool IsLoaded(Object asset) =>
        asset is TAsset assetT && ContainsAsset(assetT);

    public void Dispose()
    {
        _isDisposed = true;

        foreach (var pair in _handleToAsset)
        {
            if (pair.Asset != null)
                Addressables.Release(pair.Handle);
        }

        _handleToAsset.Clear();
    }

    public void Release(Object asset)
    {
        if (_isDisposed)
            return;

        if (asset == null) return; //mb it redundant?
        //error?
        if (asset is not TAsset instanceT)
            return;

        if (!TryPopHandle(instanceT, out AsyncOperationHandle<TAsset> handle))
            return;

        Addressables.Release(handle);
    }

    private bool TryPopHandle(TAsset asset, out AsyncOperationHandle<TAsset> handle)
    {
        foreach (var pair in _handleToAsset)
        {
            if (pair.Asset == asset)
            {
                handle = pair.Handle;
                _handleToAsset.Remove(pair);
                return true;
            }
        }

        handle = default;
        return false;
    }

    private bool ContainsAsset(TAsset asset)
    {
        foreach (var pair in _handleToAsset)
        {
            if (pair.Asset == asset)
                return true;
        }

        return false;
    }
}