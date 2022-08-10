﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.AddressableAssets;

//To inject dependencies, it should be Scoped with static instances
public sealed class GlobalAddressablesLoader : AddressablesLoader, IGlobalAddressablesLoader
{
    private static readonly Dictionary<Type, object> TypeToHandleStorage = new();
    private static readonly Dictionary<GameObject, GameObject> InstanceToPrefab = new();

    public GlobalAddressablesLoader(ICreator creator, IInjector injector) :
        base(creator, injector, TypeToHandleStorage, InstanceToPrefab) { }
}
