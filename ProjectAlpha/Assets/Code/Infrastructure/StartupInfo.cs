using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code;

//todo helpers namespace?
public static class StartupInfo
{
    //public static bool IsGameScene { get; private set; }
    //public static bool IsBootstrapScene { get; private set; }
    //public static bool IsBootstrapper { get; private set; }

    public static string StartupSceneName { get; private set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void Init()
    {
        StartupSceneName = SceneManager.GetActiveScene().name;

        //IsGameScene = StartupSceneName == SceneAddress.Game.Key;
        //IsBootstrapScene = StartupSceneName == SceneAddress.Bootstrap.Key;
        //IsBootstrapper = StartupSceneName == "Bootstrapper";
    }
}
