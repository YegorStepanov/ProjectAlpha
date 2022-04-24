using Code.Project;
using UnityEngine;
using Zenject;

namespace Code.Menu
{
    public sealed class MenuInitializer : IInitializable
    {
        private readonly SceneLoader sceneLoader;
        private readonly MenuMediator mediator;

        public MenuInitializer(SceneLoader sceneLoader, MenuMediator mediator)
        {
            this.sceneLoader = sceneLoader;
            this.mediator = mediator;
        }

        public void Initialize()
        {
            Debug.Log("MenuInitializer.Initialize" + ": " + Time.frameCount);
            // sceneLoader.LoadGameAsync();
            mediator.Open<MainMenu>();
        }
    }
}