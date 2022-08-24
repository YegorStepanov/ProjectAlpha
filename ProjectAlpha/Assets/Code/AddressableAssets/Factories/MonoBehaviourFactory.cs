using UnityEngine;

namespace Code.AddressableAssets;

public sealed class MonoBehaviourFactory<TValue> : BehaviourFactory<TValue> where TValue : MonoBehaviour
{
    public MonoBehaviourFactory(IObjectCreator creator, TValue prefab, string name, string parentName)
        : base(creator, prefab, name, parentName) { }
}
