using Code.AddressableAssets;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace Code.VContainer;

public class AddressableAssetPool<T> : AsyncPool<T> where T : Object
{
    private readonly Address<T> _address;
    private readonly IScopedAddressablesLoader _loader;

    public AddressableAssetPool(Address<T> address, PoolData data, IScopedAddressablesLoader loader) :
        base(data.InitialSize, data.Capacity)
    {
        //IsAsignable ye?((
        Assert.IsFalse(typeof(MonoBehaviour).IsAssignableFrom(typeof(T)), $"Use {nameof(AddressableComponentPool<Component>)} for GameObject");
        Assert.IsFalse(typeof(Component).IsAssignableFrom(typeof(T)), $"Use {nameof(AddressableComponentPool<Component>)} for MonoBehaviour");

        _address = address;
        _loader = loader;
    }

    protected override UniTask<T> CreateAsync()
    {
        // Object.Instantiate();
        return _loader.LoadAssetAsync(_address);
    }

    protected override void OnSpawned(T instance) { }
    protected override void OnDespawned(T instance) { }
}