using UnityEngine.SceneManagement;

namespace Code.Services;

public sealed class StartSceneInformer
{
    public bool IsGameStartScene { get; }
    public bool IsBootstrapStartScene { get; }

    public StartSceneInformer()
    {
#if UNITY_EDITOR
        IsGameStartScene = SceneManager.GetActiveScene().name == SceneAddress.Game.Key;
        IsBootstrapStartScene = SceneManager.GetActiveScene().name == SceneAddress.Bootstrap.Key;
#else
        IsGameStartScene = false;
        IsBootstrapStartScene = true;
#endif
    }
}