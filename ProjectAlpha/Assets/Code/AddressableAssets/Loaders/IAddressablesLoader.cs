using System;
using Cysharp.Threading.Tasks;
using Object = UnityEngine.Object;

namespace Code.AddressableAssets;

public interface IAddressablesLoader : IDisposable
{
    public ICreator Creator { get; }
    UniTask<T> InstantiateAsync<T>(Address<T> address, bool inject = true) where T : Object;
    UniTask<T> LoadAssetAsync<T>(Address<T> address) where T : Object;
    bool IsLoaded<T>(T instance) where T : Object;
    public void Release<T>(T instance) where T : Object;
}