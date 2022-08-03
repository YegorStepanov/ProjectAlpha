using System;
using JetBrains.Annotations;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Code.Extensions;

public static class VContainerExtensions
{
    [PublicAPI]
    public static IContainerBuilder RegisterComponentAndInjectGameObject<T>(this IContainerBuilder builder, T instance)
    {
        builder.RegisterComponent(instance);
        builder.InjectGameObject(instance);
        return builder;
    }

    [PublicAPI]
    public static IContainerBuilder InjectGameObject<T>(this IContainerBuilder builder)
    {
        return builder.InjectGameObject(typeof(T));
    }

    [PublicAPI]
    public static IContainerBuilder InjectGameObject<T>(this IContainerBuilder builder, T instance)
    {
        builder.RegisterBuildCallback(resolver =>
        {
            var resolved = resolver.Resolve<T>();

            if (resolved is not Component component)
                throw new ArgumentException($"The type must be a component, actual type is {resolved.GetType().Name}");

            resolver.InjectGameObject(component.gameObject);
        });

        return builder;
    }

    [PublicAPI]
    public static IContainerBuilder Inject<T>(this IContainerBuilder builder)
    {
        builder.RegisterBuildCallback(resolver =>
        {
            var resolved = resolver.Resolve<T>();

            if (resolved is not Component component)
                throw new ArgumentException($"The type must be a component, actual type is {resolved.GetType().Name}");

            resolver.Inject(component);
        });

        return builder;
    }

    [PublicAPI]
    public static IContainerBuilder Inject<T>(this IContainerBuilder builder, T instance)
    {
        return builder.Inject<T>();
    }

    //AsImplementedInterfaces() maybe not working, hide return value
    public static void RegisterNonLazy<T>(this IContainerBuilder builder, Lifetime lifetime)
    {
        builder.NonLazy<T>();
        builder.Register<T>(lifetime);
    }

    public static void RegisterNonLazy<TInterface, TImplement>(this IContainerBuilder builder, Lifetime lifetime)
        where TImplement : TInterface
    {
        builder.NonLazy<TInterface>();
        builder.Register<TInterface, TImplement>(lifetime);
    }

    private static void NonLazy<T>(this IContainerBuilder builder)
    {
        builder.RegisterBuildCallback(resolver => resolver.Resolve<T>());
    }
}
