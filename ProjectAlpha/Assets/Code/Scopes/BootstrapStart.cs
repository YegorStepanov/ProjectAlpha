﻿using System.Threading;
using Code.Services;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace Code.Scopes;

public sealed class BootstrapStart : IAsyncStartable
{
    private readonly SceneLoader _sceneLoader;
    private readonly LoadingScreen _loadingScreen;

    public BootstrapStart(SceneLoader sceneLoader, LoadingScreen loadingScreen)
    {
        _sceneLoader = sceneLoader;
        _loadingScreen = loadingScreen;
    }

    public async UniTask StartAsync(CancellationToken token)
    {
        _loadingScreen.Show();

        await _sceneLoader.LoadAsync<GameScene>(token);
        await _sceneLoader.LoadAsync<MenuScene>(token);

        await _loadingScreen.HideAsync();
        await _sceneLoader.UnloadAsync<BootstrapScene>(token);
    }
}