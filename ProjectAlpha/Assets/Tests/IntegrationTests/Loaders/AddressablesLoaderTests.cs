using System.Collections;
using Code.AddressableAssets;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.TestTools;

namespace Code.IntegrationTests;

public sealed class AddressablesLoaderTests : AddressableTestFixture
{
    [UnitySetUp]
    public IEnumerator Init() => UniTask.ToCoroutine(async () =>
    {
        await Addressables.InitializeAsync();
    });

    [UnityTest]
    public IEnumerator Loader_loads_and_releases_the_assets() => UniTask.ToCoroutine(async () =>
    {
        using var loader = new AddressablesLoader(new FakeCreator());

        var prefab = await loader.LoadAssetAsync(GameObjectAddress);

        Assert.That(loader.IsLoaded(prefab), Is.True);
        loader.Release(prefab);
        Assert.That(loader.IsLoaded(prefab), Is.False);

        var asset = await loader.LoadAssetAsync(AssetAddress);

        Assert.That(loader.IsLoaded(asset), Is.True);
        loader.Release(asset);
        Assert.That(loader.IsLoaded(asset), Is.False);
    });

    [UnityTest]
    public IEnumerator Loader_instantiate_and_release_the_assets() => UniTask.ToCoroutine(async () =>
    {
        using var loader = new AddressablesLoader(new FakeCreator());

        var prefab = await loader.InstantiateNoInjectAsync(GameObjectAddress);

        Assert.That(loader.IsLoaded(prefab), Is.True);
        loader.Release(prefab);
        Assert.That(loader.IsLoaded(prefab), Is.False);

        var monoBehaviour = await loader.InstantiateNoInjectAsync(MonoBehaviourAddress);

        Assert.That(loader.IsLoaded(monoBehaviour), Is.True);
        loader.Release(monoBehaviour);
        Assert.That(loader.IsLoaded(monoBehaviour), Is.False);
    });

    [UnityTest]
    public IEnumerator Loader_is_disposed() => UniTask.ToCoroutine(async () =>
    {
        using var loader = new AddressablesLoader(new FakeCreator());

        var prefab = await loader.InstantiateNoInjectAsync(GameObjectAddress);

        Assert.That(loader.IsLoaded(prefab), Is.True);
        loader.Dispose();
        Assert.That(loader.IsLoaded(prefab), Is.False);
    });

    [UnityTest]
    public IEnumerator Loader_returns_null_after_dispose() => UniTask.ToCoroutine(async () =>
    {
        using var loader = new AddressablesLoader(new FakeCreator());
        loader.Dispose();

        Assert.That(await loader.InstantiateNoInjectAsync(GameObjectAddress), Is.Null);
        Assert.That(await loader.InstantiateNoInjectAsync(MonoBehaviourAddress), Is.Null);

        Assert.That(await loader.LoadAssetAsync(GameObjectAddress), Is.Null);
        Assert.That(await loader.LoadAssetAsync(AssetAddress), Is.Null);
    });
}

public sealed class FakeCreator : ICreator
{
    public GameObject Instantiate(string name) => new(name);

    public GameObject Instantiate(GameObject prefab) => Object.Instantiate(prefab);

    public GameObject InstantiateEmpty(string name)
    {
        throw new System.NotImplementedException();
    }

    public T Instantiate<T>(T prefab) where T : Object
    {
        throw new System.NotImplementedException();
    }

    public T Instantiate<T>(T prefab, Vector3 position, Quaternion rotation) where T : Object
    {
        throw new System.NotImplementedException();
    }

    public T Instantiate<T>(T prefab, Vector3 position, Quaternion rotation, Transform parent) where T : Object
    {
        throw new System.NotImplementedException();
    }

    public T Instantiate<T>(T prefab, Transform parent, bool worldPositionStays = false) where T : Object
    {
        throw new System.NotImplementedException();
    }

    public T InstantiateNoInject<T>(T prefab) where T : Object
    {
        throw new System.NotImplementedException();
    }

    public T InstantiateNoInject<T>(T prefab, Vector3 position, Quaternion rotation) where T : Object
    {
        throw new System.NotImplementedException();
    }

    public T InstantiateNoInject<T>(T prefab, Vector3 position, Quaternion rotation, Transform parent) where T : Object
    {
        throw new System.NotImplementedException();
    }

    public T InstantiateNoInject<T>(T prefab, Transform parent, bool worldPositionStays = false) where T : Object
    {
        throw new System.NotImplementedException();
    }
}
