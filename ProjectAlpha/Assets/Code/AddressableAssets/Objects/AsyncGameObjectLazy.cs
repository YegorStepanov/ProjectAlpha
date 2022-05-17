using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.AddressableAssets;

public sealed class AsyncGameObjectLazy : AsyncObject<GameObject>, IAsyncObject<GameObject>
{
    private readonly Address<GameObject> _address;
    private readonly IScopedAddressablesLoader _loader;

    public AsyncGameObjectLazy(Address<GameObject> address, IScopedAddressablesLoader loader) : base(loader)
    {
        _address = address;
        _loader = loader;
    }

    protected override async UniTask WaitResource()
    {
        resource = await _loader.InstantiateAsync(_address);
    }
}