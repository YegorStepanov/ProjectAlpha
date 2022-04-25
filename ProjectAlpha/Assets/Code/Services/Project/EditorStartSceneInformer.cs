using UnityEngine.SceneManagement;

namespace Code.Services
{
    public sealed class EditorStartSceneInformer
    {
        public bool IsGameStartScene { get; }

        public EditorStartSceneInformer() =>
            IsGameStartScene = GetIsGameStartScene();

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