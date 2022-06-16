using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.AddressableAssets;

// to inject dependencies, it should be Scoped with static instances
public class GlobalAddressablesLoader : AddressablesLoader, IGlobalAddressablesLoader
{
    private static readonly Dictionary<Type, object> TypeToHandleStorage = new();
    private static readonly Dictionary<GameObject, GameObject> InstanceToPrefab = new();

    private protected GlobalAddressablesLoader(ICreator creator) :
        base(creator, TypeToHandleStorage, InstanceToPrefab) { }
}