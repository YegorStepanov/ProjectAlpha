using System.Collections;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.TestTools;

namespace Code.IntegrationTests;

public sealed class TestAssetLoaderTests
{
    private AddressableTestFixture _tester;

    [SetUp]
    public void SetUp()
    {
        Resources.UnloadUnusedAssets();
        _tester = new AddressableTestFixture();
    }

    [TearDown]
    public void TearDown() =>
        _tester.Dispose();

    [UnityTest]
    public IEnumerator LoadAsset() => UniTask.ToCoroutine(async () =>
    {
        await _tester.GameObject.LoadAsset();
        await _tester.Asset.LoadAsset();
    });

    [UnityTest]
    public IEnumerator LoadAsset_and_release() => UniTask.ToCoroutine(async () =>
    {
        var go = await _tester.GameObject.LoadAsset();
        Addressables.Release(go);

        var asset = await _tester.Asset.LoadAsset();
        Addressables.Release(asset);
    });

    [UnityTest]
    public IEnumerator LoadAssetHandle() => UniTask.ToCoroutine(async () =>
    {
        await _tester.GameObject.LoadAssetHandle();
        await _tester.Asset.LoadAssetHandle();
    });

    [UnityTest]
    public IEnumerator LoadAssetHandle_and_release() => UniTask.ToCoroutine(async () =>
    {
        var go = await _tester.GameObject.LoadAssetHandle();
        Addressables.Release(go);

        var asset = await _tester.Asset.LoadAssetHandle();
        Addressables.Release(asset);
    });


    [UnityTest]
    public IEnumerator Instantiate() => UniTask.ToCoroutine(async () =>
    {
        await _tester.GameObject.Instantiate();
        await _tester.MonoBehaviour.Instantiate();
        // await _tester.MonoBehaviour.Instantiate();
    });

    [UnityTest]
    public IEnumerator Instantiate_and_release() => UniTask.ToCoroutine(async () =>
    {
        var go = await _tester.GameObject.Instantiate();
        Addressables.ReleaseInstance(go);

        var component = await _tester.MonoBehaviour.Instantiate();
        Addressables.ReleaseInstance(component);

        // var monoBehaviour = await _tester.MonoBehaviour.Instantiate();
        // Addressables.ReleaseInstance(monoBehaviour);
    });


    [UnityTest]
    public IEnumerator InstantiateHandle() => UniTask.ToCoroutine(async () =>
    {
        await _tester.GameObject.InstantiateHandle();
        await _tester.MonoBehaviour.InstantiateHandle();
        // await _tester.MonoBehaviour.InstantiateHandle();
    });

    [UnityTest]
    public IEnumerator InstantiateHandle_and_release() => UniTask.ToCoroutine(async () =>
    {
        var go = await _tester.GameObject.InstantiateHandle();
        Addressables.Release(go);

        var component = await _tester.MonoBehaviour.InstantiateHandle();
        Addressables.Release(component);

        // var monoBehaviour = await _tester.MonoBehaviour.InstantiateHandle();
        // Addressables.Release(monoBehaviour);
    });
}
