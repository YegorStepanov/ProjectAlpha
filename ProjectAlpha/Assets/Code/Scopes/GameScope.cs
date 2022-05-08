﻿using Code.Services;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Code.Scopes;

public sealed class GameScope : LifetimeScope
{
    [Required, SerializeField, AssetSelector(Filter = "t:" + nameof(WidthGenerator))]
    private WidthGenerator _widthGenerator; //split to settings and own generator?

    public HeroController _hero;

    public StickController _stick;

    [AssetsOnly]
    public PlatformController _platform;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<AddressableFactory>(Lifetime.Singleton);

        builder.Register<GameStateMachine>(Lifetime.Singleton);
        
        builder.RegisterComponentInNewPrefab(_hero, Lifetime.Singleton)
            .As<IHeroController>();
                    
        ////pool, 2 instance, under STICKS go
        builder.RegisterComponentInNewPrefab(_stick, Lifetime.Singleton)
            .As<IStickController>()     
            .AsSelf();
        
        builder.RegisterComponentInNewPrefab(_platform, Lifetime.Singleton)
            .As<IPlatformController>()
            .AsSelf();
        
        builder.RegisterComponent(Instantiate(_widthGenerator));
        
        builder.RegisterEntryPoint<GameStart>();
        
        builder.Register<StickSpawner>(Lifetime.Singleton);
        builder.Register<PlatformSpawner>(Lifetime.Singleton);
        
        // builder.RegisterInstance(transform);
        ////
        //
        builder.RegisterInstance(this.GetCancellationTokenOnDestroy());

        //builder.RegisterBuildCallback(resolver =>
        //{
        //    var stateMachine = resolver.Resolve<GameStateMachine>();
        //    resolver.Inject(stateMachine);
        //});

        //////////////////


        // builder.RegisterComponentInHierarchy<YourBehaviour>()
        //     .UnderTransform();

        // Container.
        //     Debug.Log("Configure");
        // builder.Register<HelloWorldService>(Lifetime.Singleton);


        //We do not recommend excecuting Inject directly on MonoBehaviours;
        //instead, use InjectGameObject on the owning GameObject.

        //RegisterComponent is similar to RegisterInstance.
        //The only difference is that MonoBehaviours registered with RegisterComponent
        //will be injected even if not Resolved.

        //use SCOPE instead baseclass for 


        // builder.RegisterEntryPoint<GamePresenter>();

        // builder.RegisterComponent(helloScreen); //go


        //builder.UseEntryPoints(Lifetime.Singleton, entryPoints =>
        //{
        //    entryPoints.Add<GamePresenter>();
        //    // entryPoints.Add<OtherSingletonEntryPointA>();
        //    // entryPoints.Add<OtherSingletonEntryPointB>();
        //    // entryPoints.Add<OtherSingletonEntryPointC>();
        //})
    }
}