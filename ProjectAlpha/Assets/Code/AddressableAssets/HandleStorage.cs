using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace Code.AddressableAssets
{
    public sealed class HandleStorage<TAsset> : IDisposable where TAsset : Object
    {
        // it's not a dictionary because:
        // 1) Addressables.LoadAssetAsync() returns the same handle usually, but sometimes it returns a different one (differs by m_Version)...
        // 2) dictionary has big overhead here
        private readonly List<HandleData<TAsset>> _handleData = new();

        private bool _isDisposed;

        public UniTask<TAsset> AddAssetAsync(string address) =>
            AddAssetAsync(new Address<TAsset>(address));

        public async UniTask<TAsset> AddAssetAsync(Address<TAsset> address)
        {
            if (_isDisposed) return default;

            HandleData<TAsset> data = await LoadHandleDataAsync(address);
            _handleData.Add(data);
            return data.Asset;
        }

        public void RemoveAsset(string address) =>
            RemoveAsset(new Address<TAsset>(address));

        public void RemoveAsset(Address<TAsset> address)
        {
            if (_isDisposed) return;

            HandleData<TAsset> data = FindHandleData(address);
            RemoveHandleData(data);
        }

        public void RemoveAsset(TAsset asset)
        {
            if (_isDisposed) return;
            if (asset == null) return;

            HandleData<TAsset> data = FindHandleData(asset);
            RemoveHandleData(data);
        }

        public int CountAssets(Address<TAsset> address)
        {
            int count = 0;
            foreach (HandleData<TAsset> data in _handleData)
            {
                if (data.Address == address)
                    count++;
            }

            return count;
        }

        public bool ContainsAsset(TAsset asset)
        {
            if (_isDisposed) return false;
            if (asset == null) return false;

            foreach (HandleData<TAsset> data in _handleData)
            {
                if (data.Asset == asset)
                    return true;
            }

            return false;
        }

        public void RemoveAllAssets(Address<TAsset> address)
        {
            if (_isDisposed) return;

            for (int i = _handleData.Count - 1; i >= 0; i--)
            {
                HandleData<TAsset> data = _handleData[i];
                if (data.Address == address)
                {
                    RemoveHandleData(data);
                }
            }
        }

        public void Dispose()
        {
            _isDisposed = true;

            foreach (HandleData<TAsset> pair in _handleData)
                Addressables.Release(pair.Handle);

            _handleData.Clear();
        }

        private static async UniTask<HandleData<TAsset>> LoadHandleDataAsync(Address<TAsset> address)
        {
            AsyncOperationHandle<TAsset> handle = Addressables.LoadAssetAsync<TAsset>(address.Key);
            try
            {
                await handle;
                return new HandleData<TAsset>(handle, address);
            }
            catch
            {
                Addressables.Release(handle);
                throw;
            }
        }

        private HandleData<TAsset> FindHandleData(Address<TAsset> address)
        {
            foreach (HandleData<TAsset> data in _handleData)
            {
                if (data.Address == address)
                    return data;
            }

            throw new Exception($"Handle not found for address: {address}");
        }

        private HandleData<TAsset> FindHandleData(TAsset asset)
        {
            foreach (HandleData<TAsset> data in _handleData)
            {
                if (data.Asset == asset)
                    return data;
            }

            throw new Exception($"Handle not found for asset: {asset}");
        }

        private void RemoveHandleData(HandleData<TAsset> data)
        {
            Addressables.Release(data.Handle);
            _handleData.Remove(data);
        }
    }
}
