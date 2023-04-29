using Code.Services.Navigators;
using VContainer.Unity;

namespace Code.Scopes
{
    public sealed class BootstrapEntryPoint : IStartable
    {
        private readonly IBootstrapSceneNavigator _sceneNavigator;

        public BootstrapEntryPoint(IBootstrapSceneNavigator sceneNavigator) =>
            _sceneNavigator = sceneNavigator;

        void IStartable.Start() =>
            _sceneNavigator.NavigateToMenuAndGameScenes();
    }
}