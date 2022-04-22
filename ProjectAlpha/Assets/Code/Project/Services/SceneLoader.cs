using System;
using System.Threading;
using Code.Common;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using Zenject;

namespace Code.Project
{
    public sealed class SceneLoader
    {
        private readonly ZenjectSceneLoader sceneLoader;
        private readonly SceneReferences sceneReferences;

        public SceneLoader(ZenjectSceneLoader sceneLoader, SceneReferences sceneReferences)
        {
            this.sceneLoader = sceneLoader;
            this.sceneReferences = sceneReferences;
        }

        public UniTask LoadAsync<TScene>(CancellationToken token) where TScene : struct, IScene
        {
            SceneReference reference = Reference<TScene>();
            return LoadAsync(reference.ScenePath, token);
        }

        public UniTask UnloadAsync<TScene>(CancellationToken token) where TScene : struct, IScene
        {
            SceneReference reference = Reference<TScene>();
            return UnloadAsync(reference.ScenePath, token);
        }

        public async UniTask LoadAsync(string sceneName, CancellationToken token) =>
            await sceneLoader.LoadSceneAsync(sceneName, LoadSceneMode.Additive, containerMode: LoadSceneRelationship.Child).WithCancellation(token);

        public async UniTask UnloadAsync(string sceneName, CancellationToken token) =>
            await SceneManager.UnloadSceneAsync(sceneName).WithCancellation(token);

        private SceneReference Reference<TScene>() where TScene : struct, IScene => typeof(TScene) switch
        {
            Type t when t == typeof(BootstrapScene) => sceneReferences.BootstrapScene,
            Type t when t == typeof(MenuScene) => sceneReferences.MenuScene,
            Type t when t == typeof(GameScene) => sceneReferences.GameScene,
            Type t when t == typeof(MiniGameScene) => sceneReferences.MiniGameScene,
            _ => throw new ArgumentOutOfRangeException(typeof(TScene).FullName)
        };
    }

    public interface IScene { }

    public struct BootstrapScene : IScene { }

    public struct MenuScene : IScene { }

    public struct GameScene : IScene { }

    public struct MiniGameScene : IScene { }
}