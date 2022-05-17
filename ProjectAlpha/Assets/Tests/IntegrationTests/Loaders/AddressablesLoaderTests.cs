﻿using System.Collections;
using Code.AddressableAssets;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.TestTools;
using VContainer.Unity;

namespace Tests;

public sealed class AddressablesLoaderTests: AddressableTestFixture
{
    private LifetimeScope _scope;
    
    [UnitySetUp]
    public IEnumerator Init() => UniTask.ToCoroutine(async () =>
    {
        await Addressables.InitializeAsync();
        _scope = new GameObject().AddComponent<LifetimeScope>();
    });

    [TearDown]
    public void TearDown() =>
        _scope.Dispose();

    [UnityTest]
    public IEnumerator Loader_loads_and_releases_the_assets() => UniTask.ToCoroutine(async () =>
    {
        using var loader = new AddressablesLoader(_scope);
        
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
        using var loader = new AddressablesLoader(_scope);
        
        var prefab = await loader.InstantiateAsync(GameObjectAddress);

        Assert.That(loader.IsLoaded(prefab), Is.True);
        loader.Release(prefab);
        Assert.That(loader.IsLoaded(prefab), Is.False);
        
        var monoBehaviour = await loader.InstantiateAsync(MonoBehaviourAddress);

        Assert.That(loader.IsLoaded(monoBehaviour), Is.True);
        loader.Release(monoBehaviour);
        Assert.That(loader.IsLoaded(monoBehaviour), Is.False);
    });
    
    [UnityTest]
    public IEnumerator Loader_is_disposed() => UniTask.ToCoroutine(async () =>
    {
        var loader = new AddressablesLoader(_scope);
        
        var prefab = await loader.InstantiateAsync(GameObjectAddress);

        Assert.That(loader.IsLoaded(prefab), Is.True);
        loader.Dispose();
        Assert.That(loader.IsLoaded(prefab), Is.False);
    });
    
    [UnityTest]
    public IEnumerator Loader_returns_null_after_dispose() => UniTask.ToCoroutine(async () =>
    {
        var loader = new AddressablesLoader(_scope);
        loader.Dispose();

        Assert.That(await loader.InstantiateAsync(GameObjectAddress), Is.Null);
        Assert.That(await loader.InstantiateAsync(MonoBehaviourAddress), Is.Null);
        
        Assert.That(await loader.LoadAssetAsync(GameObjectAddress), Is.Null);
        Assert.That(await loader.LoadAssetAsync(AssetAddress), Is.Null);
    });
}