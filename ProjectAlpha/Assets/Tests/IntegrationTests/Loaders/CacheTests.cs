using System.Collections;
using Code.AddressableAssets;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.TestTools;

namespace Code.IntegrationTests
{
    public sealed class CacheTests
    {
        [UnitySetUp]
        public IEnumerator SetUp() => UniTask.ToCoroutine(async () =>
        {
            await Addressables.InitializeAsync();
        });

        [UnityTest]
        public IEnumerator Asset_is_not_cached_by_default() => UniTask.ToCoroutine(async () =>
        {
            Assert.That(await IsAssetCached(), Is.False);
        });

        [UnityTest]
        public IEnumerator Asset_is_cached_and_released() => UniTask.ToCoroutine(async () =>
        {
            using var cache = new HandleStorage<GameObject>();

            await cache.AddAssetAsync("Platform");
            Assert.That(await IsAssetCached(), Is.True);

            cache.RemoveAsset("Platform");

            //one frame sometimes is not enough
            await UniTask.NextFrame();
            await UniTask.NextFrame();

            Assert.That(await IsAssetCached(), Is.False);
        });

        [UnityTest]
        public IEnumerator Cache_is_disposed() => UniTask.ToCoroutine(async () =>
        {
            var cache = new HandleStorage<GameObject>();
            await CacheAssetAsync(cache, "Platform");
            cache.Dispose();

            Assert.That(await CacheAssetAsync(cache, "Platform"), Is.EqualTo(0));
        });

        private static async UniTask<int> CacheAssetAsync(HandleStorage<GameObject> storage, string address)
        {
            await storage.AddAssetAsync(address);
            return storage.CountAssets(new Address<GameObject>(address));
        }

        private static int ReleaseCachedAsset(HandleStorage<GameObject> storage, string address)
        {
            storage.RemoveAsset(address);
            return storage.CountAssets(new Address<GameObject>(address));
        }

        [UnityTest]
        public IEnumerator Cache_returns_the_number_of_holding_handles() => UniTask.ToCoroutine(async () =>
        {
            using var cache = new HandleStorage<GameObject>();

            Assert.That(await CacheAssetAsync(cache, "Platform"), Is.EqualTo(1));
            Assert.That(ReleaseCachedAsset(cache, "Platform"), Is.EqualTo(0));

            Assert.That(await CacheAssetAsync(cache, "Platform"), Is.EqualTo(1));
            Assert.That(await CacheAssetAsync(cache, "Platform"), Is.EqualTo(2));
            Assert.That(await CacheAssetAsync(cache, "Platform"), Is.EqualTo(3));
            cache.RemoveAllAssets(new Address<GameObject>("Platform"));
            Assert.That(await CacheAssetAsync(cache, "Platform"), Is.EqualTo(1));
        });

        private static async UniTask<bool> IsAssetCached()
        {
            int before = Time.frameCount;

            var handle = LoadHandle();
            await handle;
            ReleaseHandle(handle);

            int frames = Time.frameCount - before;
            return frames == 0;
        }

        private static AsyncOperationHandle<GameObject> LoadHandle() =>
            Addressables.LoadAssetAsync<GameObject>("Platform");

        private static void ReleaseHandle<T>(AsyncOperationHandle<T> handle) =>
            Addressables.Release(handle);
    }
}
