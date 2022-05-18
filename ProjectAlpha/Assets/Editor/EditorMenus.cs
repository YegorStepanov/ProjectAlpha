using UnityEditor;

namespace Code.Editor;

public static class EditorMenus
{
    [MenuItem("Tools/Clear addressables cache", false, 50)]
    public static void ClearAddressablesCache()
    {
        UnityEngine.Caching.ClearCache();
    }
}