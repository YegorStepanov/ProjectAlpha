using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.AddressableAssets;

public interface IAddressablesLoader : IDisposable
{
    UniTask<T> InstantiateAsync<T>(Address<T> address, Transform under = null) where T : Component;
    UniTask<GameObject> InstantiateAsync(Address<GameObject> address, Transform under = null);
    UniTask<T> LoadAssetAsync<T>(Address<T> address) where T : Object;
    bool IsLoaded<T>(T instance) where T : Object;
    public void Release<T>(T instance) where T : Object;
}