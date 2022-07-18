using UnityEngine;
using VContainer;

namespace Code.VContainer;

public sealed class MonoBehaviourFactory<TValue> : BehaviourFactory<TValue> where TValue : MonoBehaviour
{
    public MonoBehaviourFactory(ICreator creator, IObjectResolver resolver, TValue prefab, InstanceName name, ParentName parentName)
        : base(creator, resolver, prefab, name, parentName) { }
}
