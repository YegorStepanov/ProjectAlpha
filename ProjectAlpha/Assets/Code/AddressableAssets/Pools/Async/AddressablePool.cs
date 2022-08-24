using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.AddressableAssets;

public sealed class AddressablePool<TComponent> : AsyncPool<TComponent> where TComponent : Component
{
    private const string ContainerPostfix = " Pool";

    private readonly Address<TComponent> _address;
    private readonly IAddressablesLoader _loader;

    private Transform _container;

    public AddressablePool(Address<TComponent> address, int initialSize, int capacity, IAddressablesLoader loader)
        : base(initialSize, capacity)
    {
        _address = address;
        _loader = loader;
    }

    protected override async UniTask<TComponent> CreateAsync()
    {
        if (_container == null)
            _container = _loader.Creator.InstantiateEmpty(_address.Key + ContainerPostfix).transform;

        TComponent instance = await _loader.InstantiateAsync(_address);
        instance.transform.parent = _container;

        return instance;
    }

    protected override void OnSpawned(TComponent instance) =>
        instance.gameObject.SetActive(true);

    protected override void OnDespawned(TComponent instance) =>
        instance.gameObject.SetActive(false);
}
