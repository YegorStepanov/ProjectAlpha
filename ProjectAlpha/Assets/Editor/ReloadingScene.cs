using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Utils
{
    public static class ReloadingScene
    {
        [MenuItem("Tools/Reload Scene")]
        public static void ReloadScene()
        {
            Scene activeScene = SceneManager.GetActiveScene();
            EditorSceneManager.SaveScene(activeScene, "Assets/myscene.unity", true);
            EditorSceneManager.OpenScene(activeScene.path); 
        }

        [MenuItem("Tools/Toggle Domain Reloading")]
        public static void ToggleDomainReloading()
        {
            EnterPlayModeOptions options = EditorSettings.enterPlayModeOptions;

            if (options.HasFlag(EnterPlayModeOptions.DisableDomainReload))
            {
                options &= ~EnterPlayModeOptions.DisableDomainReload;
                Debug.Log("Domain reloading enabled");
            }
            else
            {
                options |= EnterPlayModeOptions.DisableDomainReload;
                Debug.Log("Domain reloading disabled");
            }

            EditorSettings.enterPlayModeOptions = options;
            AssetDatabase.RefreshSettings();
        }
    }
}