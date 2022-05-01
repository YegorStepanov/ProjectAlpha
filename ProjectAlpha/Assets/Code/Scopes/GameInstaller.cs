﻿using Code.Game;
using Code.Services;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Code.Scopes;

public sealed class GameInstaller : BaseInstaller<GameInitializer>
{
    [Required, SerializeField, AssetSelector(Filter = "t:" + nameof(WidthGenerator))]
    private WidthGenerator _widthGenerator; //split to settings and own generator?

    [Required, SerializeField, AssetSelector(Filter = "t:" + nameof(GameSettings))]
    private GameSettings _gameSettings;

    public HeroController _hero;

    public StickController _stick;

    [AssetsOnly]
    public PlatformController _platform;

    public override void InstallBindings()
    {
        base.InstallBindings();


        RegisterWidthGenerator();

        RegisterGameSettings();

        RegisterGameStateMachine();

        RegisterHeroController();

        RegisterPlatformControllerPool();

        RegisterStickController();

        RegisterPlatformSpawner();

        RegisterStickControllerPool();

        RegisterStickSpawner();
    }

    private void RegisterStickSpawner() =>
        Container.Bind<StickSpawner>().AsSingle();

    private void RegisterStickControllerPool() =>
        Container.BindMemoryPool<StickController, StickController.Pool>()
            .WithInitialSize(2)
            .FromComponentInNewPrefab(_stick)
            .WithGameObjectName("Stick")
            .UnderTransformGroup("Sticks");

    private void RegisterPlatformSpawner() =>
        Container.Bind<PlatformSpawner>().AsSingle();

    private void RegisterStickController()
    {
        Container.Bind<IStickController>()
            .FromComponentInNewPrefab(_stick)
            .WithGameObjectName("Stick")
            .AsSingle();
    }

    private void RegisterPlatformControllerPool()
    {
        Container.BindMemoryPool<PlatformController, PlatformController.Pool>()
            .WithInitialSize(5)
            .FromComponentInNewPrefab(_platform)
            .WithGameObjectName("Platform")
            .UnderTransformGroup("Platforms");
    }

    private void RegisterHeroController() =>
        Container.Bind<IHeroController>()
            .FromComponentInNewPrefab(_hero)
            .WithGameObjectName("Hero")
            .AsSingle();

    private void RegisterGameStateMachine() =>
        Container.Bind<GameStateMachine>().AsSingle();

    private void RegisterGameSettings() =>
        _gameSettings.BindAllSettings(Container);

    private void RegisterWidthGenerator() =>
        Container.Bind<WidthGenerator>()
            .To<PercentageWidthGenerator>()
            .FromNewScriptableObject(_widthGenerator)
            .AsSingle();
}