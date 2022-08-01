using Code.AddressableAssets;
using JetBrains.Annotations;
using UnityEngine;

namespace Code.Extensions;

// ReSharper disable once InconsistentNaming
public static class IAddressablesLoaderExtensions
{
    [PublicAPI]
    public static IAsyncPool<T> CreateAsyncPool<T>(
        this IAddressablesLoader loader, Address<T> address, int initialSize, int capacity)
        where T : Component
    {
        AddressablePool<T> pool = new(address, initialSize, capacity, loader);
        return pool;
    }

    public static IAsyncPool<T> CreateAsyncCyclicPool<T>(
        this IAddressablesLoader loader, Address<T> address, int initialSize, int capacity)
        where T : Component
    {
        IAsyncPool<T> pool = CreateAsyncPool(loader, address, initialSize, capacity);
        return new RecyclablePool<T>(pool);
    }
}
