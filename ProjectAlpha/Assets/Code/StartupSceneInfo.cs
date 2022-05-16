using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code;

public static class StartupSceneInfo
{
    public static bool IsStartedInGameStartScene { get; private set; }
    public static bool IsStartedInBootstrapScene { get; private set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void Init()
    {
        IsStartedInGameStartScene = SceneManager.GetActiveScene().name == SceneAddress.Game.Key;
        IsStartedInBootstrapScene = SceneManager.GetActiveScene().name == SceneAddress.Bootstrap.Key;
    }
}