using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

namespace Code.Services;

public sealed class SceneLoader
{
    private readonly IObjectResolver _resolver;
    private readonly ScopedAddressableLoader _loader;

    private readonly Dictionary<Address<Scene>, SceneInstance> _scenes = new();

    private readonly string _startupSceneName;

    public SceneLoader(IObjectResolver resolver, ScopedAddressableLoader loader)
    {
        _resolver = resolver;
        _loader = loader;
        _startupSceneName = SceneManager.GetActiveScene().name;
    }

    /// <summary>
    /// Do not load scene concurrently
    /// </summary>
    public async UniTask LoadAsync<TScene>(LifetimeScope parentScope, CancellationToken token) where TScene : struct, IScene
    {
        Address<Scene> address = GetAddress<TScene>();
        await LoadAsync(address, parentScope, token);
    }

    public UniTask UnloadAsync<TScene>(CancellationToken token) where TScene : struct, IScene
    {
        Address<Scene> address = GetAddress<TScene>();
        return UnloadAsync(address, token);
    }

    private async UniTask LoadAsync(Address<Scene> sceneAddress, LifetimeScope parentScope, CancellationToken token)
    {
        if (parentScope != null)
        {
            using (LifetimeScope.EnqueueParent(parentScope))
            {
                SceneInstance scene = await Addressables.LoadSceneAsync(sceneAddress.Key, LoadSceneMode.Additive);
                _scenes.Add(sceneAddress, scene);
            }
        }
        else
        {
            SceneInstance scene = await Addressables.LoadSceneAsync(sceneAddress.Key, LoadSceneMode.Additive);
            _scenes.Add(sceneAddress, scene);
        }
    }

    private async UniTask UnloadAsync(Address<Scene> sceneAddress, CancellationToken token)
    {
        if (_scenes.TryGetValue(sceneAddress, out SceneInstance scene))
        {
            _scenes.Remove(sceneAddress);
            await Addressables.UnloadSceneAsync(scene).WithCancellation(token);
        }
        else
        {
            //todo: find a better solution
            await SceneManager.UnloadSceneAsync(_startupSceneName);
        }
    }

    private static Address<Scene> GetAddress<TScene>() where TScene : struct, IScene => typeof(TScene) switch
    {
        Type t when t == typeof(BootstrapScene) => SceneAddress.Bootstrap,
        Type t when t == typeof(MenuScene) => SceneAddress.Menu,
        Type t when t == typeof(GameScene) => SceneAddress.Game,
        Type t when t == typeof(MiniGameScene) => SceneAddress.MiniGame,
        _ => throw new ArgumentOutOfRangeException(typeof(TScene).FullName)
    };
}

public interface IScene { }

public struct BootstrapScene : IScene { }

public struct MenuScene : IScene { }

public readonly record struct GameScene : IScene { }

public struct MiniGameScene : IScene { }