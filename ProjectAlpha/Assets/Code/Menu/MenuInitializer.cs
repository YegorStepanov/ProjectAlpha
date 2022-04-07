using Code.Project;
using UnityEngine;
using Zenject;

namespace Code.Menu
{
    public sealed class MenuInitializer : IInitializable
    {
        private readonly SceneLoader sceneLoader;

        public MenuInitializer(SceneLoader sceneLoader)
        {
            this.sceneLoader = sceneLoader;
        }

        public void Initialize()
        {
            Debug.Log("MenuInitializer.Initialize" + ": " + Time.frameCount);
            // sceneLoader.LoadGameAsync();
        }
    }
}