using Code.Project;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Code.Menu
{
    public class AddressableFactory
    {
        private readonly DiContainer container;
        private readonly Transform sceneTransform;

        public AddressableFactory(DiContainer container, Transform sceneTransform)
        {
            this.container = container;
            this.sceneTransform = sceneTransform;
        }

        public async UniTask<GameObject> InstantiateAsync(Address address)
        {
            GameObject go = await Addressables.InstantiateAsync(address.Key, sceneTransform);
            go.transform.SetParent(null);
            go.name = address.Key;
            container.InjectGameObject(go);
            return go;
        }

        public void ReleaseInstance(GameObject go) =>
            Addressables.ReleaseInstance(go);
    }
}