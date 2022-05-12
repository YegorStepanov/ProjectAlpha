using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using VContainer.Unity;

namespace Code.Services;

//it cannot be used because there is one root dependencies
//rename to Loader
public sealed class GlobalAddressableLoader : ScopedAddressableLoader
{
    private static readonly Dictionary<Object, HandleData> assetsToHandle = new();
    private static readonly Dictionary<GameObject, GameObject> instanceToPrefab = new();
    private static readonly Dictionary<AddressData, AsyncOperationHandle> rootedDataToPreloadedAsset = new();
    
    public GlobalAddressableLoader(LifetimeScope scope) :
        base(scope, assetsToHandle, instanceToPrefab, rootedDataToPreloadedAsset) { }

    public override void Dispose()
    {
        base.Dispose();
        assetsToHandle.Clear();
        instanceToPrefab.Clear();
        rootedDataToPreloadedAsset.Clear();
    }
}