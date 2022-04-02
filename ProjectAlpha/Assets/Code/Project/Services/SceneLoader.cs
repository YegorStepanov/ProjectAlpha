using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using FluentAssertions;
using Sirenix.OdinInspector;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Zenject;

namespace Code.Project
{
    public sealed class SceneLoader : IInitializable
    {
        private readonly ZenjectSceneLoader sceneLoader;
        private readonly Scenes scenes;

        private List<Scene> loadedScenes;

        public SceneLoader(ZenjectSceneLoader sceneLoader, Scenes scenes)
        {
            this.sceneLoader = sceneLoader;
            this.scenes = scenes;
        }

        // public void Initialize() =>
        //     InitSceneList();


        public UniTask LoadMenuAsync(Action onLoaded = null) =>
            LoadSceneAdditiveAsync(scenes.MenuScene.name);

        public UniTask LoadGameAsync(Action onLoaded = null) =>
            LoadSceneAdditiveAsync(scenes.GameScene.name);

        public UniTask LoadMiniGameAsync(Action onLoaded = null) =>
            LoadSceneAdditiveAsync(scenes.MiniGameScene.name);

        private UniTask LoadSceneAdditiveAsync(string sceneName) =>
            sceneLoader.LoadSceneAsync(sceneName, LoadSceneMode.Additive).ToUniTask();

        public void Initialize() { }
    }
}