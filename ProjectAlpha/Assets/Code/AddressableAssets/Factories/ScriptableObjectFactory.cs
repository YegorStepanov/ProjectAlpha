using UnityEngine;

namespace Code.AddressableAssets
{
    public sealed class ScriptableObjectFactory<TValue> : BehaviourFactory<TValue> where TValue : ScriptableObject
    {
        public ScriptableObjectFactory(IObjectCreator creator, TValue prefab, string name, string parentName)
            : base(creator, prefab, name, parentName) { }
    }
}