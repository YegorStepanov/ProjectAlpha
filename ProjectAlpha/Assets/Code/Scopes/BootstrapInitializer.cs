﻿using System.Threading;
using Code.Services;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Code.Scopes
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
            loadingScreen.Show();

            UniTask menuLoading = sceneLoader.LoadAsync<MenuScene>(token);
            UniTask gameLoading = sceneLoader.LoadAsync<GameScene>(token);

            await (menuLoading, gameLoading);

            await loadingScreen.HideAsync();
            await sceneLoader.UnloadAsync<BootstrapScene>(token);
        }
    }
}