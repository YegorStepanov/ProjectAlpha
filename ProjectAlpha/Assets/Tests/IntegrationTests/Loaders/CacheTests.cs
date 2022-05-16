using System.Collections;
using Code.AddressableAssets;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.TestTools;

namespace Tests
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
            using var cache = new AddressableAssetCache<GameObject>();

            await cache.CacheAssetAsync("Platform");

            Assert.That(await IsAssetCached(), Is.True);

            cache.ReleaseCachedAsset("Platform");

            await UniTask.NextFrame();
            
            Assert.That(await IsAssetCached(), Is.False);
        });
        
        [UnityTest]
        public IEnumerator Cache_is_disposed() => UniTask.ToCoroutine(async () =>
        {
            var cache = new AddressableAssetCache<GameObject>();
            await cache.CacheAssetAsync("Platform");
            cache.Dispose();
            
            Assert.That(await cache.CacheAssetAsync("Platform"), Is.EqualTo(1));
        });

        [UnityTest]
        public IEnumerator Cache_returns_the_number_of_holding_handles() => UniTask.ToCoroutine(async () =>
        {
            using var cache = new AddressableAssetCache<GameObject>();

            Assert.That(await cache.CacheAssetAsync("Platform"), Is.EqualTo(1));
            Assert.That(cache.ReleaseCachedAsset("Platform"), Is.EqualTo(0));
            Assert.That(cache.ReleaseCachedAsset("Platform"), Is.EqualTo(-1));

            Assert.That(await cache.CacheAssetAsync("Platform"), Is.EqualTo(1));
            Assert.That(await cache.CacheAssetAsync("Platform"), Is.EqualTo(2));
            Assert.That(await cache.CacheAssetAsync("Platform"), Is.EqualTo(3));
            cache.RemoveCachedAsset("Platform");
            Assert.That(await cache.CacheAssetAsync("Platform"), Is.EqualTo(1));
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