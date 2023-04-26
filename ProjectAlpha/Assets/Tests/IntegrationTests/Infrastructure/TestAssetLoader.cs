using System.Collections.Generic;
using Code.AddressableAssets;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Code.IntegrationTests
{
    public sealed class TestAssetLoader<T> : ITestGameObjectLoader<T> where T : Object
    {
        private readonly Address<T> _address;

        private readonly List<AsyncOperationHandle<T>> _loadGameObjectAsset = new();
        private readonly List<AsyncOperationHandle<T>> _loadGameObjectAssetHandle = new();
        private readonly List<GameObject> _instantiateGameObject = new();
        private readonly List<AsyncOperationHandle<GameObject>> _instantiateGameObjectHandle = new();

        public TestAssetLoader(Address<T> address) =>
            _address = address;

        public void Dispose()
        {
            //remove additional checks?

            foreach (var asset in _loadGameObjectAsset)
            {
                if (asset.IsValid())
                    Addressables.Release(asset);
            }

            foreach (var handle in _loadGameObjectAssetHandle)
            {
                if (handle.IsValid())
                    Addressables.Release(handle);
            }

            foreach (var go in _instantiateGameObject)
            {
                if (go != null)
                    Addressables.Release(go);
            }

            foreach (var handle in _instantiateGameObjectHandle)
            {
                if (handle.IsValid())
                    Addressables.Release(handle);
            }
        }

        public async UniTask<T> LoadAsset()
        {
            var asset = Addressables.LoadAssetAsync<T>(_address.Key);
            _loadGameObjectAsset.Add(asset);

            return await asset;
        }

        public async UniTask<AsyncOperationHandle<T>> LoadAssetHandle()
        {
            var handle = Addressables.LoadAssetAsync<T>(_address.Key);
            _loadGameObjectAssetHandle.Add(handle);

            await handle;
            return handle;
        }

        public async UniTask<GameObject> Instantiate()
        {
            var go = await Addressables.InstantiateAsync(_address.Key);
            _instantiateGameObject.Add(go);

            return go;
        }

        public async UniTask<AsyncOperationHandle<GameObject>> InstantiateHandle()
        {
            var handle = Addressables.InstantiateAsync(_address.Key, trackHandle: false);
            _instantiateGameObjectHandle.Add(handle);

            await handle;
            return handle;
        }
    }
}
