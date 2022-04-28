using UnityEngine.SceneManagement;

namespace Code.Services
{
    public sealed class StartSceneInformer
    {
        //without ctor it evaluate property lazy? Why?
        public bool IsGameStartScene { get; } = GetIsGameStartScene();

        private static bool GetIsGameStartScene()
        {
#if UNITY_EDITOR
            return SceneManager.GetActiveScene().name == SceneAddress.Game.Key;
#else
            return false;
#endif
        }
    }
}