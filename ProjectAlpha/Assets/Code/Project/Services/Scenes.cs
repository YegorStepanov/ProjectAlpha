using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using Zenject;
using Debug = UnityEngine.Debug;

namespace Code.Project
{
    [CreateAssetMenu(menuName = "SO/Scenes")]
    public sealed class Scenes : ScriptableObject, IInitializable
    {
        [field: SerializeField]
        private string MenuSceneName;

        [field: SerializeField]
        private string GameSceneName;

        [field: SerializeField]
        private string MiniGameSceneName;

        private Scene? menuScene;
        private Scene? gameScene;
        private Scene? miniGameScene;

        public Scene MenuScene => menuScene ??= GetScene(MenuSceneName);
        public Scene GameScene => gameScene ??= GetScene(GameSceneName);
        public Scene MiniGameScene => miniGameScene ??= GetScene(MiniGameSceneName);


        [NonSerialized]
        private Scene[] allScenes;

        public Scene[] BuildScenes => allScenes ??= GetAllScenes();
        
        private static Scene[] GetAllScenes()
        {
            var scenes = new Scene[SceneManager.sceneCountInBuildSettings];
            Debug.Log(SceneManager.sceneCount);
            for (int i = 0; i < scenes.Length; i++)
            {
                scenes[i] = SceneManager.GetSceneByBuildIndex(i);
                string o = SceneUtility.GetScenePathByBuildIndex(i);
                Debug.Log($"full: {o}\nname: {Path.GetFileNameWithoutExtension(o)}");
            }
            return scenes;
        }

        void IInitializable.Initialize()
        {
#if UNITY_EDITOR
            Debug.Log("UNITY_EDITOR");
#endif

#if DEBUG
            Debug.Log("DEBUG");
#endif

            return;
#if UNITY_EDITOR

#if DEBUG
            NewFunction(MenuScene);
            NewFunction(GameScene);
            NewFunction(MiniGameScene);
#endif
            Debug.Log("Validation is over");
#endif
            Debug.Log("Validation is over2");

            void NewFunction(Scene menuScene)
            {
                menuScene.IsValid();
            }
        }

        private Scene GetScene(string sceneName)
        {
            allScenes = null;
            Scene scene = BuildScenes.FirstOrDefault(scene => scene.name == sceneName);
            // Scene scene = SceneManager.GetSceneByName(sceneName);
 
            Assert.IsTrue(scene.IsValid(), $"The scene with a name `{sceneName}` cannot be found");
            return scene;
        }
    }
}