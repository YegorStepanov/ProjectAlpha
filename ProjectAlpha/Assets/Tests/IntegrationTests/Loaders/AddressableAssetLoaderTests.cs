using System.Collections;
using Code.AddressableAssets;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Code.IntegrationTests;

public sealed class AddressableAssetLoaderTests : AddressableTestFixture
{
    [UnityTest]
    public IEnumerator Loader_loads_and_releases_an_asset() => UniTask.ToCoroutine(async () =>
    {
        using var loader = new HandleStorage<GameObject>();

        var asset = await loader.AddAssetAsync("Platform");

        Assert.That(loader.ContainsAsset(await GameObject.LoadAsset()), Is.True);

        loader.RemoveAsset(asset);

        Assert.That(loader.ContainsAsset(await GameObject.LoadAsset()), Is.False);
    });

    [UnityTest]
    public IEnumerator Loader_is_disposed() => UniTask.ToCoroutine(async () =>
    {
        var loader = new HandleStorage<GameObject>();
        await loader.AddAssetAsync("Platform");

        loader.Dispose();

        Assert.That(loader.ContainsAsset(await GameObject.LoadAsset()), Is.False);
    });
}
