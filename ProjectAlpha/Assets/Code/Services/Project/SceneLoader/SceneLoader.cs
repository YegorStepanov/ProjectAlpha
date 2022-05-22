using System;
using System.Collections.Generic;
using System.Threading;
using Code.AddressableAssets;
using Code.Scopes;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using VContainer.Unity;

namespace Code.Services;

public sealed class SceneLoader : ISceneLoader
{
    private readonly Dictionary<Address<Scene>, SceneInstance> _scenes = new();
    private readonly List<GameObject> _tempRootGameObjects = new(32);

    public static ISceneLoader Instance { get; private set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void Init() =>
        Instance = new SceneLoader();

    private SceneLoader() { }

    public UniTask LoadAsync<TScene>(CancellationToken token) where TScene : struct, IScene
    {
        return LoadCoreAsync(Address<TScene>(), token);
    }

    public async UniTask LoadAsync<TScene>(LifetimeScope parentScope, CancellationToken token)
        where TScene : struct, IScene
    {
        using (LifetimeScope.EnqueueParent(parentScope))
            await LoadCoreAsync(Address<TScene>(), token);
    }

    public UniTask UnloadAsync<TScene>(CancellationToken token) where TScene : struct, IScene
    {
        return UnloadCoreAsync(Address<TScene>(), token);
    }

    private async UniTask LoadCoreAsync(Address<Scene> address, CancellationToken token)
    {
        SceneInstance scene = await Addressables.LoadSceneAsync(address.Key, LoadSceneMode.Additive)
            .WithCancellation(token);

        _scenes.Add(address, scene);

        if (TryGetScope(scene, out Scope scope))
            await scope.OnPreloadedAsync();
        else
            Debug.LogError("The first object in scene should be Scope");
    }

    private async UniTask UnloadCoreAsync(Address<Scene> address, CancellationToken token)
    {
        if (_scenes.TryGetValue(address, out SceneInstance scene))
        {
            _scenes.Remove(address);
            await Addressables.UnloadSceneAsync(scene).WithCancellation(token);
        }
        else
        {
            //todo: find a better solution
            await SceneManager.UnloadSceneAsync(StartupSceneInfo.SceneName); //StartupSceneInfo.SceneName or address.Key
        }
    }

    private static Address<Scene> Address<TScene>() where TScene : struct, IScene => typeof(TScene) switch
    {
        Type t when t == typeof(BootstrapScene) => SceneAddress.Bootstrap,
        Type t when t == typeof(MenuScene) => SceneAddress.Menu,
        Type t when t == typeof(GameScene) => SceneAddress.Game,
        Type t when t == typeof(MiniGameScene) => SceneAddress.MiniGame,
        _ => throw new ArgumentOutOfRangeException(typeof(TScene).FullName)
    };

    private bool TryGetScope(SceneInstance scene, out Scope scope)
    {
        scene.Scene.GetRootGameObjects(_tempRootGameObjects);
        GameObject first = _tempRootGameObjects[0];

        return first.TryGetComponent(out scope);
    }
}

public interface IScene { }

public struct BootstrapScene : IScene { }

public struct MenuScene : IScene { }

public struct GameScene : IScene { }

public struct MiniGameScene : IScene { }