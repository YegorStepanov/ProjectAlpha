﻿using System.Collections.Generic;
using Code.AddressableAssets;
using Code.Extensions;
using Code.Services;
using Code.Services.Data;
using Code.Services.Development;
using Code.Services.Entities;
using Code.Services.Infrastructure;
using Code.Services.Monetization;
using Code.UI;
using Cysharp.Threading.Tasks;
using MessagePipe;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Event = Code.Common.Event;
using Progress = Code.Services.Data.Progress;

namespace Code.Scopes;

public sealed class RootScope : Scope
{
    private ICamera _camera;
    private SettingsFacade _settingsFacade;
    private DevelopmentPanel _developmentPanel;
    private ILoadingScreen _loadingScreen;

    protected override async UniTask PreloadAsync(IAddressablesLoader loader)
    {
        (_camera, _settingsFacade, _loadingScreen, _) = await (
            loader.InstantiateAsync(Address.Infrastructure.CameraController, inject: false),
            loader.LoadAssetAsync(Address.Infrastructure.Settings),
            loader.InstantiateAsync(Address.UI.TransitionLoadingScreen, inject: false),
            loader.InstantiateAsync(Address.Infrastructure.EventSystem, inject: false));

        await LoadDevelopmentAssets(loader, _settingsFacade);
    }

    private async UniTask LoadDevelopmentAssets(IAddressablesLoader loader, SettingsFacade settings)
    {
        if (PlatformInfo.IsDevelopment && settings.Development.GraphyInDebug)
            loader.InstantiateAsync(Address.Development.Graphy, inject: false).Forget();

        if (PlatformInfo.IsDevelopment)
            _developmentPanel = await loader.InstantiateAsync(Address.Development.Panel, inject: false);
    }

    protected override void ConfigureServices(IContainerBuilder builder)
    {
        builder.Register<HeroSelector>(Lifetime.Singleton);
        builder.RegisterInstance<IReadOnlyList<Address<Hero>>>(new[]
        {
            Address.Entity.Hero.Hero1,
            Address.Entity.Hero.Hero2,
            Address.Entity.Hero.Hero3,
            Address.Entity.Hero.Hero4,
        });

        RegisterSettings(builder);
        RegisterCamera(builder);

        RegisterInput(builder);
        builder.Register<IRandomizer, Randomizer>(Lifetime.Singleton);

        RegisterSceneLoader(builder);
        RegisterLoaders(builder);
        RegisterAds(builder);
        RegisterIAP(builder);

        RegisterProgress(builder);
        RegisterMessagePipe(builder);
        RegisterDevelopment(builder);
        RegisterLoadingScreen(builder);

        builder.RegisterEntryPoint<RootEntryPoint>();
    }

    private void RegisterSettings(IContainerBuilder builder)
    {
        _settingsFacade.RegisterSettings(builder);
    }

    private void RegisterCamera(IContainerBuilder builder)
    {
        builder.RegisterComponent(_camera);

        builder.RegisterInstance(_camera.Background);
        builder.Inject(_camera.Background);
        builder.RegisterInstance<IReadOnlyList<Address<Texture2D>>>(
            new List<Address<Texture2D>>
            {
                Address.Background.Background1,
                Address.Background.Background2,
                Address.Background.Background3,
                Address.Background.Background4,
                Address.Background.Background5,
            });
    }

    private static void RegisterInput(IContainerBuilder builder)
    {
        builder.Register<IInputManager, InputManager>(Lifetime.Singleton);
    }

    private static void RegisterSceneLoader(IContainerBuilder builder)
    {
        builder.Register<ISceneLoader, SceneLoader>(Lifetime.Singleton);
    }

    private static void RegisterLoaders(IContainerBuilder builder)
    {
        builder.Register<ICreator, Creator>(Lifetime.Scoped);
        builder.Register<IAddressablesCache, AddressablesCache>(Lifetime.Scoped);
        builder.Register<IScopedAddressablesLoader, AddressablesLoader>(Lifetime.Scoped);
        builder.Register<IGlobalAddressablesLoader, GlobalAddressablesLoader>(Lifetime.Scoped);
    }

    private static void RegisterAds(IContainerBuilder builder)
    {
        builder.Register<BannerAd>(Lifetime.Transient).AsImplementedInterfaces().AsSelf();

        builder.Register<InterstitialAd>(Lifetime.Transient);
        builder.Register<RewardedAd>(Lifetime.Transient);

        builder.Register<AdInitializer>(Lifetime.Transient);
        builder.Register<IAds, Ads>(Lifetime.Singleton);

        builder.Register<AdsManager>(Lifetime.Singleton);
    }

    private static void RegisterIAP(IContainerBuilder builder)
    {
        builder.Register<IIAPManager, IAPManager>(Lifetime.Singleton);
    }

    private static void RegisterProgress(IContainerBuilder builder)
    {
        builder.Register<IPersistentProgress, PersistentProgress>(Lifetime.Singleton);
        builder.Register<ISessionProgress, SessionProgress>(Lifetime.Singleton);
        builder.Register<IProgress, Progress>(Lifetime.Singleton);
    }

    private static void RegisterMessagePipe(IContainerBuilder builder)
    {
        MessagePipeOptions options = builder.RegisterMessagePipe();
        //Setup GlobalMessagePipe to enable diagnostics window and global function
        builder.RegisterBuildCallback(c => GlobalMessagePipe.SetProvider(c.AsServiceProvider()));
        builder.RegisterMessageBroker<Event.GameStart>(options);
    }

    private void RegisterDevelopment(IContainerBuilder builder)
    {
        if (PlatformInfo.IsDevelopment)
        {
            builder.RegisterNonLazy<DevelopmentRootPanel>(Lifetime.Singleton);
            builder.RegisterComponentAndInjectGameObject(_developmentPanel);
        }
    }

    private void RegisterLoadingScreen(IContainerBuilder builder)
    {
        builder.RegisterInstance(_loadingScreen);
    }
}
