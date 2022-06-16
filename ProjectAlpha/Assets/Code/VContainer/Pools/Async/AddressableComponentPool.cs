using Code.AddressableAssets;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.VContainer;

public sealed class AddressableComponentPool<TComponent> : AsyncPool<TComponent> where TComponent : Component
{
    private readonly ICreator _creator;
    private readonly Address<TComponent> _address;
    private readonly string _containerName;
    private readonly IScopedAddressablesLoader _loader;

    private Transform _container;

    public AddressableComponentPool(
        ICreator creator,
        Address<TComponent> address,
        ComponentPoolData data,
        IScopedAddressablesLoader loader)
        : base(data.InitialSize, data.Capacity)
    {
        _creator = creator;
        _address = address;
        _containerName = data.ContainerName;
        _loader = loader;
    }

    protected override UniTask<TComponent> CreateAsync()
    {
        if (_container == null)
            _container = _creator.Instantiate(_containerName).transform;

        return _loader.InstantiateAsync(_address);
    }

    protected override void OnSpawned(TComponent instance) =>
        instance.gameObject.SetActive(true);

    protected override void OnDespawned(TComponent instance) =>
        instance.gameObject.SetActive(false);
}

public record PoolData(int InitialSize, int Capacity);

public sealed record ComponentPoolData(string ContainerName, int InitialSize, int Capacity) :
    PoolData(InitialSize, Capacity);