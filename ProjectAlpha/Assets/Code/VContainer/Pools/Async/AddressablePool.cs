using Code.Services;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer.Unity;

namespace Code.VContainer;

public class AddressablePool<TComponent> : AsyncPool<TComponent> where TComponent : Component
{
    private readonly Address<TComponent> _address;
    private readonly string _containerName;
    private readonly ScopedAddressableLoader _loader;
    private readonly LifetimeScope _scope;

    private Transform _container;

    protected AddressablePool(Address<TComponent> address, PoolData data, ScopedAddressableLoader loader,
        LifetimeScope scope)
        : base(data.InitialSize, data.Capacity)
    {
        _address = address;
        _containerName = data.ContainerName;
        _scope = scope;
        _loader = loader;
    }

    protected override async UniTask<TComponent> CreateAsync()
    {
        if (_container == null)
            _container = _scope.CreateRootSceneContainer(_containerName);

        return await _loader.LoadAsync(_address);
    }

    protected override void OnSpawned(TComponent instance) =>
        instance.gameObject.SetActive(true);

    protected override void OnDespawned(TComponent instance) =>
        instance.gameObject.SetActive(false);
}