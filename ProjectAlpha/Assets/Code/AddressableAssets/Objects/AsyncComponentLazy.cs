using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.AddressableAssets;

public sealed class AsyncComponentLazy<T> : AsyncObject<T>, IAsyncObject<T> where T : Component
{
    private readonly Address<T> _address;
    private readonly IScopedAddressablesLoader _loader;

    public AsyncComponentLazy(Address<T> address, IScopedAddressablesLoader loader) : base(loader)
    {
        _address = address;
        _loader = loader;
    }

    protected override async UniTask WaitResource()
    {
        resource = await _loader.InstantiateAsync(_address);
    }
}