using Code.AddressableAssets;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.VContainer;

public sealed class AddressableComponentPool<TComponent> : AsyncPool<TComponent> where TComponent : Component
{
    private readonly Address<TComponent> _address;
    private readonly string _containerName;
    private readonly IAddressablesLoader _loader;

    private Transform _container;

    public AddressableComponentPool(
        Address<TComponent> address,
        int initialSize,
        int capacity,
        string containerName,
        IAddressablesLoader loader)
        : base(initialSize, capacity)
    {
        _address = address;
        _containerName = containerName;
        _loader = loader;
    }

    protected override UniTask<TComponent> CreateAsync()
    {
        if (_container == null)
            _container = _loader.Creator.Instantiate(_containerName).transform;

        return _loader.InstantiateAsync(_address);
    }

    protected override void OnSpawned(TComponent instance) =>
        instance.gameObject.SetActive(true);

    protected override void OnDespawned(TComponent instance) =>
        instance.gameObject.SetActive(false);
}
