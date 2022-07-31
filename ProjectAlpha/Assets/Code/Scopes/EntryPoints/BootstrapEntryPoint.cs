﻿using System.Threading;
using Code.Services.Infrastructure;
using Code.UI;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace Code.Scopes;

public sealed class BootstrapEntryPoint : IAsyncStartable
{
    private readonly ISceneLoader _sceneLoader;
    private readonly LoadingScreen _loadingScreen;

    public BootstrapEntryPoint(ISceneLoader sceneLoader, LoadingScreen loadingScreen)
    {
        _sceneLoader = sceneLoader;
        _loadingScreen = loadingScreen;
    }

    async UniTask IAsyncStartable.StartAsync(CancellationToken token)
    {
        _loadingScreen.Show();

        await _sceneLoader.LoadAsync(Address.Scene.Game, token);
        await _sceneLoader.LoadAsync(Address.Scene.Menu,token);

        await _loadingScreen.FadeOutAsync();
        await _sceneLoader.UnloadAsync(Address.Scene.Bootstrap, token);
    }
}
