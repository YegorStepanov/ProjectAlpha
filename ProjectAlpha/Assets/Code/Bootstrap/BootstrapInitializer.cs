using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Code.Project
{
    public sealed class BootstrapInitializer : IInitializable
    {
        private readonly SceneLoader sceneLoader;
        private readonly LoadingScreen loadingScreen;
        private readonly CancellationToken token;

        public BootstrapInitializer(SceneLoader sceneLoader, LoadingScreen loadingScreen, CancellationToken token)
        {
            this.sceneLoader = sceneLoader;
            this.loadingScreen = loadingScreen;
            this.token = token;
        }

        public void Initialize() =>
            InitializeAsync().Forget();

        private async UniTaskVoid InitializeAsync()
        {
            Debug.Log("BootstrapInitializer.Initialize" + ": " + Time.frameCount);
            
            loadingScreen.Show();

            UniTask menuLoading = sceneLoader.LoadAsync<MenuScene>(token);
            UniTask gameLoading = sceneLoader.LoadAsync<GameScene>(token);

            await (menuLoading, gameLoading);

            await loadingScreen.HideAsync();
            await sceneLoader.UnloadAsync<BootstrapScene>(token);
        }
    }
}