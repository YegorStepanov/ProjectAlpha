using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;
using Zenject;

namespace Code.Services
{
    public class AddressableFactory: IDisposable
    {
        private readonly DiContainer container;
        private readonly Transform installerTransform;

        //rename
        private readonly Dictionary<object, object> assetToHandle = new();
        
        
        public AddressableFactory(DiContainer container, Transform installerTransform)
        {
            this.container = container;
            this.installerTransform = installerTransform;
            // todo: Addressables.InitializeAsync() somewhere
        }

        public async UniTask<GameObject> InstantiateAsync(Address address)
        {
            GameObject go = await Addressables.InstantiateAsync(address.Key, installerTransform);
            go.transform.SetParent(null);
            go.name = address.Key;
            container.InjectGameObject(go);
            return go; //.GetComponent()
        }

        //todo: add LoadAssetExplicitAsync?
        //it should works with GameObjects(by InstantiateAsync)?
        public async UniTask<T> LoadAssetAsync<T>(Address address) where T : class
        {
            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(address.Key);
            T asset = await handle;
            assetToHandle.Add(asset, handle);
            return asset;
        }

        [SuppressMessage("ReSharper", "MemberCanBeMadeStatic.Global")]
        public void ReleaseInstance(GameObject go) =>
            Addressables.ReleaseInstance(go);

        public void ReleaseAsset<T>(T asset) where T : class
        {
            if (asset == null) return;
            
            var handle = (AsyncOperationHandle<T>)assetToHandle[asset];
            assetToHandle.Remove(asset);
            Addressables.Release(handle);
        }

        public void Dispose()
        {
            //replace to .Values
            foreach (object handle in assetToHandle.Values) 
                Addressables.Release(handle);
            
            assetToHandle.Clear();
        }
    }
}