using Code.AddressableAssets;
using UnityEngine;

namespace Code.Extensions;

// ReSharper disable once InconsistentNaming
public static class IAddressablesLoaderExtensions
{
    public static IAsyncPool<T> CreatePool<T>(
        this IAddressablesLoader loader, Address<T> address, int initialSize, int capacity, string containerName)
        where T : Component
    {
        AddressableComponentPool<T> pool = new(address, initialSize, capacity, containerName, loader);
        return pool;
    }

    public static IAsyncPool<T> CreateCyclicPool<T>(
        this IAddressablesLoader loader, Address<T> address, int initialSize, int capacity, string containerName)
        where T : Component
    {
        IAsyncPool<T> pool = CreatePool(loader, address, initialSize, capacity, containerName);
        return new RecyclablePool<T>(pool);
    }
}
