﻿using Code.AddressableAssets;
using Code.Services;
using Code.VContainer;
using Cysharp.Threading.Tasks;
using VContainer;
using VContainer.Unity;

namespace Code.Scopes;

public sealed class GameScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        RegisterPlatformSpawner(builder);
        RegisterStickSpawner(builder);
        RegisterHeroSpawner(builder);

        builder.Register<GameStateMachine>(Lifetime.Singleton);

        RegisterWidthGenerator(builder);

        builder.RegisterEntryPoint<GameStart>();

        builder.RegisterInstance(this.GetCancellationTokenOnDestroy());
        
        //RegisterComponent = RegisterInstance + Resolve NonLazy?
    }

    private void RegisterWidthGenerator(IContainerBuilder builder)
    {
        Address<WidthGenerator> platformAddress = new("Width Generator");

        builder.RegisterAddressableAssetPool(platformAddress, 0, 1, Lifetime.Singleton)            
            .AsImplementedInterfaces();   
        
        // builder.RegisterComponent(Instantiate(_widthGenerator));
        
        builder.Register                                                                                        
            <IAsyncRecyclablePool<WidthGenerator>, RecyclablePool<WidthGenerator>>                      
            (Lifetime.Singleton);          

        builder.Register<WidthGeneratorSpawner>(Lifetime.Singleton);
    }

    private static void RegisterHeroSpawner(IContainerBuilder builder)
    {
        builder.RegisterAddressablePool(GameAddress.Hero, "Heroes", 0, 1, Lifetime.Singleton)            
            .AsImplementedInterfaces();                                                                         
                                                                                                          
        builder.Register                                                                                        
            <IAsyncRecyclablePool<HeroController>, RecyclablePool<HeroController>>                      
            (Lifetime.Singleton);                                                                               
                                                                                                          
        builder.Register<HeroSpawner>(Lifetime.Singleton);
    }

    private static void RegisterPlatformSpawner(IContainerBuilder builder)
    {
        builder.RegisterAddressablePool(GameAddress.Platform, "Platforms", 0, 3, Lifetime.Singleton)
            .AsImplementedInterfaces();

        builder.Register
            <IAsyncRecyclablePool<PlatformController>, RecyclablePool<PlatformController>>
            (Lifetime.Singleton);
        
        builder.Register<PlatformSpawner>(Lifetime.Singleton);
    }

    private static void RegisterStickSpawner(IContainerBuilder builder)
    {
        builder.RegisterAddressablePool(GameAddress.Stick, "Sticks", 0, 2, Lifetime.Singleton)
            .AsImplementedInterfaces();

        builder.Register
            <IAsyncRecyclablePool<StickController>, RecyclablePool<StickController>>
            (Lifetime.Singleton);
        
        builder.Register<StickSpawner>(Lifetime.Singleton);
    }
}