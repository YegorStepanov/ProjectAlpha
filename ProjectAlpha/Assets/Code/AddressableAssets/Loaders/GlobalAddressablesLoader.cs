using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace Code.AddressableAssets;

public class GlobalAddressablesLoader : AddressablesLoader, IGlobalAddressablesLoader
{
    private static readonly Dictionary<Type, IAddressableAssetLoader<Object>> loaders = new();
    private static readonly Dictionary<GameObject, GameObject> instanceToPrefab = new();

    private protected GlobalAddressablesLoader(LifetimeScope scope) :
        base(scope, loaders, instanceToPrefab) { }
}