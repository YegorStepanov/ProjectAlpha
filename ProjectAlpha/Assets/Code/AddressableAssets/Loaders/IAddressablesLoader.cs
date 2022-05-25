using System;
using Code.VContainer;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.AddressableAssets;

public interface IAddressablesLoader : IDisposable
{
    UniTask<T> InstantiateAsync<T>(Address<T> address, Transform under = null, bool inject = true) where T : Object;
    UniTask<T> LoadAssetAsync<T>(Address<T> address) where T : Object;
    bool IsLoaded<T>(T instance) where T : Object;
    public void Release<T>(T instance) where T : Object;
    public IAsyncPool<T> CreatePool<T>(Address<T> address, int size, string container) where T : Component;
    public IAsyncPool<T> CreateCyclicPool<T>(Address<T> address, int size, string container) where T : Component;
}