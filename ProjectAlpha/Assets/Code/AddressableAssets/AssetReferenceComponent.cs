using Code.Infrastructure;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Code.AddressableAssets;

[System.Serializable]
public class AssetReferenceComponent<T> : AssetReferenceGameObject where T : Component
{
    public AssetReferenceComponent(string guid) : base(guid) { }

    public override bool ValidateAsset(Object obj)
    {
        var go = obj as GameObject;
        return go != null && go.GetComponent<T>() != null;
    }

    public override bool ValidateAsset(string path)
    {
        if (PlatformInfo.IsPlayer)
            return false;

        var go = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(path);
        return go != null && go.GetComponent<T>() != null;
    }

    public static implicit operator Address<T>(AssetReferenceComponent<T> reference) =>
        new((string)reference.RuntimeKey);
}
