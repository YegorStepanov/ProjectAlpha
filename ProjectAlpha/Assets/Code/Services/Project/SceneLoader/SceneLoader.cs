using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using Zenject;

namespace Code.Services;

public sealed class SceneLoader
{
    private readonly AddressableZenjectSceneLoader sceneLoader;

    private readonly Dictionary<Address, SceneInstance> scenes = new();

    private readonly string startupSceneName;

    public SceneLoader([InjectOptional] SceneContext sceneRoot)
    {
        sceneLoader = new AddressableZenjectSceneLoader(sceneRoot);
        startupSceneName = SceneManager.GetActiveScene().name;
    }

    public UniTask LoadAsync<TScene>(CancellationToken token) where TScene : struct, IScene =>
        LoadAsync(GetAddress<TScene>(), token);

    public UniTask UnloadAsync<TScene>(CancellationToken token) where TScene : struct, IScene =>
        UnloadAsync(GetAddress<TScene>(), token);

    private async UniTask LoadAsync(Address sceneAddress, CancellationToken token)
    {
        SceneInstance scene = await sceneLoader.LoadSceneAdditiveAsync(sceneAddress.Key).WithCancellation(token);
        scenes.Add(sceneAddress, scene);
    }

    private async UniTask UnloadAsync(Address sceneAddress, CancellationToken token)
    {
        if (scenes.TryGetValue(sceneAddress, out SceneInstance scene))
        {
            scenes.Remove(sceneAddress);
            await Addressables.UnloadSceneAsync(scene).WithCancellation(token);
        }
        else
        {
            //todo: find a better solution
            await SceneManager.UnloadSceneAsync(startupSceneName);
        }
    }

    private static Address GetAddress<TScene>() where TScene : struct, IScene => typeof(TScene) switch
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

public struct GameScene : IScene { }

public struct MiniGameScene : IScene { }