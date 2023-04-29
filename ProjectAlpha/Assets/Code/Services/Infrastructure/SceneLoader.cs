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
using Scene = Code.Common.Scene;

namespace Code.Services.Infrastructure
{
    public sealed class SceneLoader : ISceneLoader
    {
        private readonly Dictionary<Address<Scene>, List<SceneInstance>> _scenes = new();
        private readonly List<GameObject> _tempRootGameObjects = new(32);
        private readonly CancellationToken _token;

        public SceneLoader(CancellationToken token) =>
            _token = token;

        public bool IsLoaded<TScene>() where TScene : struct, IScene
        {
            Address<Scene> scene = GetAddress<TScene>();
            return _scenes.ContainsKey(scene);
        }

        public UniTask LoadAsync<TScene>() where TScene : struct, IScene
        {
            Address<Scene> scene = GetAddress<TScene>();
            return LoadImpl(scene);
        }

        public async UniTask LoadAsync<TScene>(LifetimeScope parentScope) where TScene : struct, IScene
        {
            Address<Scene> scene = GetAddress<TScene>();
            using (LifetimeScope.EnqueueParent(parentScope))
                await LoadImpl(scene);
        }

        public UniTask UnloadAsync<TScene>() where TScene : struct, IScene
        {
            Address<Scene> scene = GetAddress<TScene>();
            return UnloadImpl(scene);
        }

        private async UniTask LoadImpl(Address<Scene> address)
        {
            SceneInstance scene = await Addressables.LoadSceneAsync(address.Key, LoadSceneMode.Additive)
                .WithCancellation(_token);

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

        private UniTask UnloadImpl(Address<Scene> address)
        {
            if (TryPopScene(address, out SceneInstance scene))
                return Addressables.UnloadSceneAsync(scene).WithCancellation(_token);

            if (address.Key == StartupInfo.StartupSceneName)
                return SceneManager.UnloadSceneAsync(StartupInfo.StartupSceneName).WithCancellation(_token);

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

        private static Address<Scene> GetAddress<TScene>() where TScene : struct, IScene => typeof(TScene) switch
        {
            Type t when t == typeof(BootstrapScene) => Address.Scene.Bootstrap,
            Type t when t == typeof(MenuScene) => Address.Scene.Menu,
            Type t when t == typeof(GameScene) => Address.Scene.Game,
            _ => throw new ArgumentOutOfRangeException(typeof(TScene).FullName)
            };
    }

    public interface IScene { }

    public struct BootstrapScene : IScene { }

    public struct MenuScene : IScene { }

    public struct GameScene : IScene { }
}