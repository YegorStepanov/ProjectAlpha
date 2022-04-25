using ModestTree;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using Zenject;

namespace Code.Services
{
    public sealed class AddressableZenjectSceneLoader
    {
        readonly DiContainer _sceneContainer;

        public AddressableZenjectSceneLoader(SceneContext sceneRoot)
        {
            if (sceneRoot != null)
                _sceneContainer = sceneRoot.Container;
        }

        public AsyncOperationHandle<SceneInstance> LoadSceneAdditiveAsync(Address sceneAddress)
        {
            PrepareForLoadScene(LoadSceneRelationship.Child);
            // Assert.That(Application.CanStreamedLevelBeLoaded(sceneName), "Unable to load scene '{0}'", sceneName);
            return Addressables.LoadSceneAsync(sceneAddress.Key, LoadSceneMode.Additive);
        }

        void PrepareForLoadScene(LoadSceneRelationship containerMode)
        {
            switch (containerMode)
            {
                case LoadSceneRelationship.None:
                case LoadSceneRelationship.Child when _sceneContainer == null:
                    SceneContext.ParentContainers = null;
                    break;
                case LoadSceneRelationship.Child:
                    SceneContext.ParentContainers = new[] { _sceneContainer };
                    break;
                case LoadSceneRelationship.Sibling:
                default:
                    Assert.IsNotNull(_sceneContainer,
                        "Cannot use LoadSceneRelationship.Sibling when loading scenes from ProjectContext");
                    Assert.IsEqual(containerMode, LoadSceneRelationship.Sibling);
                    SceneContext.ParentContainers = _sceneContainer.ParentContainers;
                    break;
            }
        }
    }
}