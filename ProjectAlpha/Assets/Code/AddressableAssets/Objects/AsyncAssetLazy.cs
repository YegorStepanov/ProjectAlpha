using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.AddressableAssets;

public sealed class AsyncAssetLazy<T> : AsyncObject<T>, IAsyncObject<T> where T : Object
{
    private readonly Address<T> _address;
    private readonly IScopedAddressablesLoader _loader;

    public AsyncAssetLazy(Address<T> address, IScopedAddressablesLoader loader) : base(loader)
    {
        if (typeof(T).IsAssignableTo(typeof(Component)))
            throw new ArgumentException("<T> must not be a Component/MonoBehaviour");

        _address = address;
        _loader = loader;
    }

    protected override async UniTask WaitResource()
    {
        resource = await _loader.LoadAssetAsync(_address);
    }
}