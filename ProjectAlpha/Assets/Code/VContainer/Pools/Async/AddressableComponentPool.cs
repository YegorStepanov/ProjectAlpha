using Code.AddressableAssets;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer.Unity;

namespace Code.VContainer;

public class AddressableComponentPool<TComponent> : AsyncPool<TComponent> where TComponent : Component
{
    private readonly Address<TComponent> _address;
    private readonly string _containerName;
    private readonly IScopedAddressablesLoader _loader;
    private readonly LifetimeScope _scope;

    private Transform _container;

    public AddressableComponentPool(
        Address<TComponent> address,
        ComponentPoolData data,
        IScopedAddressablesLoader loader,
        LifetimeScope scope)
        : base(data.InitialSize, data.Capacity)
    {
        _address = address;
        _containerName = data.ContainerName;
        _loader = loader;
        _scope = scope;
    }

    protected override UniTask<TComponent> CreateAsync()
    {
        if (_container == null)
            _container = _scope.CreateRootSceneContainer(_containerName);

        return _loader.InstantiateAsync(_address, _container);
    }

    protected override void OnSpawned(TComponent instance) =>
        instance.gameObject.SetActive(true);

    protected override void OnDespawned(TComponent instance) =>
        instance.gameObject.SetActive(false);
}