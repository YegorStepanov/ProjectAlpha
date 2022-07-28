using JetBrains.Annotations;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Code;

public static class VContainerExtensions
{
    [PublicAPI]
    public static IContainerBuilder NonLazy<T>(this IContainerBuilder builder)
    {
        builder.RegisterBuildCallback(resolver => resolver.Resolve<T>());
        return builder;
    }

    [PublicAPI]
    public static IContainerBuilder InjectGameObject<T>(this IContainerBuilder builder) where T : Component
    {
        builder.RegisterBuildCallback(resolver =>
        {
            var instance = resolver.Resolve<T>();
            resolver.InjectGameObject(instance.gameObject);
        });

        return builder;
    }

    [PublicAPI]
    public static IContainerBuilder Inject<T>(this IContainerBuilder builder) where T : Component
    {
        builder.RegisterBuildCallback(resolver =>
        {
            var instance = resolver.Resolve<T>();
            resolver.Inject(instance);
        });

        return builder;
    }

    [PublicAPI]
    public static IContainerBuilder NonLazy<T>(this IContainerBuilder builder, T instance)
    {
        builder.RegisterBuildCallback(resolver => resolver.Resolve(instance.GetType()));
        return builder;
    }
}
