using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.AddressableAssets;

public sealed class AsyncComponent<T> : AsyncObject<T>, IAsyncObject<T> where T : Component
{
    public AsyncComponent(Address<T> address, IScopedAddressablesLoader loader) : base(loader)
    {
        loader.InstantiateAsync(address).ContinueWith<T>(asset => resource = asset);
    }

    protected override UniTask WaitResource() => UniTask.WaitUntil(() => IsLoaded);
}