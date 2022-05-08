using UnityEngine;
using VContainer.Unity;

namespace Code.VContainer;

public sealed class MonoBehaviourFactory<TValue> : BehaviourFactory<TValue> where TValue : MonoBehaviour
{
    public MonoBehaviourFactory(TValue prefab, InstanceName name, ParentName parentName, LifetimeScope scope)
        : base(prefab, name, parentName, scope) { }
}