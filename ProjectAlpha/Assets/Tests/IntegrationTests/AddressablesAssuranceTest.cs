using System.Collections;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.TestTools;

namespace Code.IntegrationTests;

// Asset can be released by instance!
// Addressables.LoadAsset("x"), then Addressables.Instantiate("x") == 2 ref counts
public sealed class AddressablesAssuranceTest
{
    //[OneTimeSetUp]
    //public void Ini() =>
    //    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);


    [UnitySetUp]
    public IEnumerator Init() => UniTask.ToCoroutine(async () =>
    {
        await Addressables.InitializeAsync();
    });

    [UnityTest]
    public IEnumerator Assets_are_not_cached_by_default() => UniTask.ToCoroutine(async () =>
    {
        Assert.That(await IsAssetCached(), Is.False);

        await UniTask.NextFrame();

        Assert.That(await IsAssetCached(), Is.False);
    });

    [UnityTest]
    public IEnumerator Asset_is_cached_when_it_was_previously_loaded_by_the_handle() => UniTask.ToCoroutine(async () =>
    {
        var handle = LoadHandle();
        await handle; //!

        Assert.That(await IsAssetCached(), Is.True);

        ReleaseHandle(handle);
    });

    [UnityTest]
    public IEnumerator Asset_is_cached_when_it_was_previously_loaded_by_the_instance() => UniTask.ToCoroutine(async () =>
    {
        var instance = await InstantiateGameObject();

        Assert.That(await IsAssetCached(), Is.True);

        ReleaseGameObject(instance);
    });

    [UnityTest]
    public IEnumerator Handle_can_be_released_by_the_instance() => UniTask.ToCoroutine(async () =>
    {
        var instance = await LoadHandle();
        ReleaseGameObject(instance);
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

    private static AsyncOperationHandle<GameObject> InstantiateGameObject() =>
        Addressables.InstantiateAsync("Platform");

    private static void ReleaseGameObject(GameObject instance) =>
        Addressables.ReleaseInstance(instance);
}
