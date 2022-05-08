using UnityEngine;
using VContainer.Unity;

namespace Code.VContainer;

public sealed class ScriptableObjectFactory<TValue> : BehaviourFactory<TValue> where TValue : ScriptableObject
{
    public ScriptableObjectFactory(TValue prefab, InstanceName name, ParentName parentName, LifetimeScope scope)
        : base(prefab, name, parentName, scope) { }
}