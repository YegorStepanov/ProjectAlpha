using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.AddressableAssets;

public sealed class AsyncGameObject : AsyncObject<GameObject>, IAsyncObject<GameObject>
{
    public AsyncGameObject(Address<GameObject> address, IScopedAddressablesLoader loader) : base(loader)
    {
        loader.InstantiateAsync(address).ContinueWith<GameObject>(asset => resource = asset);
    }

    protected override UniTask WaitResource() => UniTask.WaitUntil(() => IsLoaded);
}