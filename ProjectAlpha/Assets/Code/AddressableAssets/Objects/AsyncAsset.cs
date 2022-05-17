using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.AddressableAssets;

public sealed class AsyncAsset<T> : AsyncObject<T>, IAsyncObject<T> where T : Object
{
    public AsyncAsset(Address<T> address, IScopedAddressablesLoader loader) : base(loader)
    {
        if (typeof(T).IsAssignableTo(typeof(Component)))
            throw new ArgumentException("<T> must not be a Component/MonoBehaviour");

        loader.LoadAssetAsync(address).ContinueWith<T>(asset => resource = asset);
    }

    protected override UniTask WaitResource() => UniTask.WaitUntil(() => IsLoaded);
}