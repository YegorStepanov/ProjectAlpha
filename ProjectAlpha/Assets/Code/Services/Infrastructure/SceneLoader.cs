using System.Collections.Generic;
using System.Threading;
using Code.AddressableAssets;
using Code.Common;
using Code.Scopes;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using VContainer.Unity;
using Scene = Code.Common.Scene;

namespace Code.Services.Infrastructure;

public sealed class SceneLoader : ISceneLoader
{
    private readonly Dictionary<Address<Scene>, List<SceneInstance>> _scenes = new();
    private readonly List<GameObject> _tempRootGameObjects = new(32);

    public bool IsLoaded(Address<Scene> scene) =>
        _scenes.ContainsKey(scene);

    public UniTask LoadAsync(Address<Scene> scene, CancellationToken token) =>
        LoadImpl(scene, token);

    public async UniTask LoadAsync(Address<Scene> scene, LifetimeScope parentScope, CancellationToken token)
    {
        using (LifetimeScope.EnqueueParent(parentScope))
            await LoadImpl(scene, token);
    }

    public UniTask UnloadAsync(Address<Scene> scene, CancellationToken token) =>
        UnloadImpl(scene, token);

    private async UniTask LoadImpl(Address<Scene> address, CancellationToken token)
    {
        SceneInstance scene = await Addressables.LoadSceneAsync(address.Key, LoadSceneMode.Additive)
            .WithCancellation(token);

        PushScene(address, scene);
        Scope scope = GetScope(scene);

        await scope.WaitForBuild();
    }

    private void PushScene(Address<Scene> address, SceneInstance scene)
    {
        if (_scenes.TryGetValue(address, out List<SceneInstance> list))
            list.Add(scene);
        else
            _scenes.Add(address, new List<SceneInstance> { scene });
    }

    private UniTask UnloadImpl(Address<Scene> address, CancellationToken token)
    {
        if (TryPopScene(address, out SceneInstance scene))
            return Addressables.UnloadSceneAsync(scene).WithCancellation(token);

        if (address.Key == StartupInfo.StartupSceneName)
            return SceneManager.UnloadSceneAsync(StartupInfo.StartupSceneName).WithCancellation(token);

        Debug.LogError($"Trying unload unknown scene: {address.Key}");
        return UniTask.CompletedTask;
    }

    private bool TryPopScene(Address<Scene> address, out SceneInstance scene)
    {
        if (_scenes.TryGetValue(address, out List<SceneInstance> scenes))
        {
            scene = scenes[0];
            scenes.RemoveAt(0);

            if (scenes.Count == 0)
                _scenes.Remove(address);

            return true;
        }

        scene = default;
        return false;
    }

    private Scope GetScope(SceneInstance scene)
    {
        scene.Scene.GetRootGameObjects(_tempRootGameObjects);
        GameObject first = _tempRootGameObjects[0];

        if (!first.TryGetComponent(out Scope scope))
            Debug.LogError("The first object in the scene should be Scope");

        return scope;
    }
}
