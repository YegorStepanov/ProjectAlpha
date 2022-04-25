using System.Diagnostics.CodeAnalysis;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Code.Services
{
    public class AddressableFactory
    {
        private readonly DiContainer container;
        private readonly Transform installerTransform;

        public AddressableFactory(DiContainer container, Transform installerTransform)
        {
            this.container = container;
            this.installerTransform = installerTransform;
        }

        public async UniTask<GameObject> InstantiateAsync(Address address)
        {
            GameObject go = await Addressables.InstantiateAsync(address.Key, installerTransform);
            go.transform.SetParent(null);
            go.name = address.Key;
            container.InjectGameObject(go);
            return go;
        }

        [SuppressMessage("ReSharper", "MemberCanBeMadeStatic.Global")]
        public void ReleaseInstance(GameObject go) =>
            Addressables.ReleaseInstance(go);
    }
}