﻿using UnityEngine;
using VContainer;

namespace Code.AddressableAssets;

public sealed class ScriptableObjectFactory<TValue> : BehaviourFactory<TValue> where TValue : ScriptableObject
{
    public ScriptableObjectFactory(ICreator creator, IObjectResolver resolver, TValue prefab, string name, string parentName)
        : base(creator, resolver, prefab, name, parentName) { }
}
