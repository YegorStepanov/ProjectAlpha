using UnityEngine;
using VContainer;

namespace Code.AddressableAssets;

public sealed class MonoBehaviourFactory<TValue> : BehaviourFactory<TValue> where TValue : MonoBehaviour
{
    public MonoBehaviourFactory(ICreator creator, IObjectResolver resolver, TValue prefab, string name, string parentName)
        : base(creator, resolver, prefab, name, parentName) { }
}
