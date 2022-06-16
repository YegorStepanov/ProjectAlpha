using UnityEngine;
using VContainer;

namespace Code.VContainer;

public sealed class ScriptableObjectFactory<TValue> : BehaviourFactory<TValue> where TValue : ScriptableObject
{
    public ScriptableObjectFactory(ICreator creator, IObjectResolver resolver, TValue prefab, InstanceName name, ParentName parentName)
        : base(creator, resolver, prefab, name, parentName) { }
}