using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Code.Project
{
    public sealed class ProjectInitializer : IInitializable
    {
        private readonly SceneLoader sceneLoader;

        public ProjectInitializer(SceneLoader sceneLoader) =>
            this.sceneLoader = sceneLoader;

        public void Initialize()
        {
            Debug.Log("ProjectInitializer.Initialize");

            DOTween.Init();

            sceneLoader.LoadGameAsync();
            sceneLoader.LoadMenuAsync();
        }
    }
}