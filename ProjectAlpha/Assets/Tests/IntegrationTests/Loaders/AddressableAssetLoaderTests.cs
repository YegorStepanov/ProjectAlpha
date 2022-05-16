using System.Collections;
using Code.AddressableAssets;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests;

public sealed class AddressableAssetLoaderTests: AddressableTestFixture
{
    [UnityTest]
    public IEnumerator Loader_loads_and_releases_an_asset() => UniTask.ToCoroutine(async () =>
    {
        using var loader = new AddressableAssetLoader<GameObject>();
        
        var asset = await loader.LoadAssetAsync("Platform");

        Assert.That(loader.IsLoaded(await GameObject.LoadAsset()), Is.True);
        
        loader.Release(asset);
        
        Assert.That(loader.IsLoaded(await GameObject.LoadAsset()), Is.False);
    });

    [UnityTest]
    public IEnumerator Loader_is_disposed() => UniTask.ToCoroutine(async () =>
    {
        var loader = new AddressableAssetLoader<GameObject>();
        await loader.LoadAssetAsync("Platform");
        
        loader.Dispose();
        
        Assert.That(loader.IsLoaded(await GameObject.LoadAsset()), Is.False);
    });
}