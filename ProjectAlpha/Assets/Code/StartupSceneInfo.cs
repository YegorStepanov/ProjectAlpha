using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code;

public static class StartupSceneInfo
{
    public static bool IsGameScene { get; private set; }
    public static bool IsBootstrapScene { get; private set; }
    public static bool IsBootstrapper { get; private set; }

    public static string SceneName { get; private set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void Init()
    {
        Debug.Log("StartupScene Init");
        SceneName = SceneManager.GetActiveScene().name;
        
        IsGameScene = SceneName == SceneAddress.Game.Key;
        IsBootstrapScene = SceneName == SceneAddress.Bootstrap.Key;
        IsBootstrapScene = SceneName == "Bootstrapper";
    }
}