using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code;

public static class StartupSceneInfo
{
    [PublicAPI] public static bool IsGameScene { get; private set; }
    [PublicAPI] public static bool IsBootstrapScene { get; private set; }
    [PublicAPI] public static bool IsBootstrapper { get; private set; }

    public static string SceneName { get; private set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void Init()
    {
        SceneName = SceneManager.GetActiveScene().name;

        IsGameScene = SceneName == SceneAddress.Game.Key;
        IsBootstrapScene = SceneName == SceneAddress.Bootstrap.Key;
        IsBootstrapScene = SceneName == "Bootstrapper";
    }
}