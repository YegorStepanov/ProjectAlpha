using UnityEngine;
using VContainer.Unity;

namespace Code.VContainer;

public class MonoBehaviourPool<TValue> : BehaviourPool<TValue> where TValue : MonoBehaviour
{
    protected MonoBehaviourPool(TValue prefab, InstanceName name, ParentName parentName, InitialSize initialSize, Capacity capacity, LifetimeScope scope)
        : base(prefab, name, parentName, initialSize, capacity, scope) { }

    protected override void OnSpawned(TValue instance) =>
        instance.gameObject.SetActive(true);

    protected override void OnDespawned(TValue instance) =>
        instance.gameObject.SetActive(false);
}