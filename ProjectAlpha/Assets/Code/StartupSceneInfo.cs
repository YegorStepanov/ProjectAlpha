using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code;

public static class StartupSceneInfo
{
    public static bool IsGameScene { get; private set; }
    public static bool IsBootstrapScene { get; private set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void Init()
    {
        IsGameScene = SceneManager.GetActiveScene().name == SceneAddress.Game.Key;
        IsBootstrapScene = SceneManager.GetActiveScene().name == SceneAddress.Bootstrap.Key;
    }
}