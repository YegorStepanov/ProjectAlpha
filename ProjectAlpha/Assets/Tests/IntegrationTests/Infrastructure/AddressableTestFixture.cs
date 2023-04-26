using System;
using System.Collections;
using Code.AddressableAssets;
using Code.Services.Entities;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.TestTools;

namespace Code.IntegrationTests
{
    public class AddressableTestFixture: IDisposable
    {
        private static readonly Address<GameObject> gameObjectAddress = new("Platform");
        private static readonly Address<Platform> monoBehaviourAddress = new("Platform");
        private static readonly Address<Sprite> assetAddress = new("Background 1");

        protected Address<GameObject> GameObjectAddress => gameObjectAddress;
        protected Address<Platform> MonoBehaviourAddress => monoBehaviourAddress;
        protected Address<Sprite> AssetAddress => assetAddress;

        public ITestGameObjectLoader<GameObject> GameObject { get; } = new TestAssetLoader<GameObject>(gameObjectAddress);
        public ITestComponentLoader<Platform> MonoBehaviour { get; } =  new TestAssetLoader<Platform>(monoBehaviourAddress);
        public ITestAssetLoader<Sprite> Asset { get; } = new TestAssetLoader<Sprite>(assetAddress);

        [UnitySetUp]
        public IEnumerator BaseSetUp() => UniTask.ToCoroutine(async () =>
        {
            await Addressables.InitializeAsync();
        });

        [TearDown]
        public void BaseTearDown() =>
            Dispose();

        public void Dispose()
        {
            GameObject.Dispose();
            MonoBehaviour.Dispose();
            Asset.Dispose();
        }
    }
}
