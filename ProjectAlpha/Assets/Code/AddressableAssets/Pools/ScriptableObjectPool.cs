using UnityEngine;
using VContainer.Unity;

namespace Code.AddressableAssets.Pools;

public class ScriptableObjectPool<TValue> : BehaviourPool<TValue> where TValue : ScriptableObject
{
    protected ScriptableObjectPool(TValue prefab, InstanceName name, ParentName parentName, InitialSize initialSize,
        Capacity capacity, LifetimeScope scope)
        : base(prefab, name, parentName, initialSize, capacity, scope) { }

    protected override void OnSpawned(TValue instance) { }

    protected override void OnDespawned(TValue instance) { }
}
