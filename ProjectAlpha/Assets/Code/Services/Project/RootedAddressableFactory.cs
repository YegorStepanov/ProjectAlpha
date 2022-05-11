using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using VContainer.Unity;

namespace Code.Services;

//it cannot be used because there is one root dependencies
//rename to Loader
public sealed class GlobalAddressableFactory : ScopedAddressableFactory
{
    private static readonly Dictionary<Object, AsyncOperationHandle> assetsToHandle = new();
    private static readonly Dictionary<GameObject, GameObject> instanceToPrefab = new();
    private static readonly Dictionary<AddressData, Object> rootedDataToPreloadedAsset = new();
    
    public GlobalAddressableFactory(LifetimeScope scope) :
        base(scope, assetsToHandle, instanceToPrefab, rootedDataToPreloadedAsset) { }

    public override void Dispose()
    {
        Debug.Log("Dispose ROOT");
        base.Dispose();
        assetsToHandle.Clear();
        instanceToPrefab.Clear();
        rootedDataToPreloadedAsset.Clear();
    }
}