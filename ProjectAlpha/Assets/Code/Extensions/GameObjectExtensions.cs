using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Code;

public static class GameObjectExtensions
{
    public static bool IsPrefab(this GameObject gameObject) =>
        gameObject.scene == default;
}

public static class VContainerExtensions
{
    public static IContainerBuilder NonLazy<T>(this IContainerBuilder builder)
    {
        builder.RegisterBuildCallback(resolver => resolver.Resolve<T>());
        return builder;
    }

    public static IContainerBuilder InjectGameObject<T>(this IContainerBuilder builder) where T : Component
    {
        builder.RegisterBuildCallback(resolver =>
        {
            Debug.Log("InjectedGameObject");
            var instance = resolver.Resolve<T>();
            resolver.InjectGameObject(instance.gameObject);
        });

        return builder;
    }

    public static IContainerBuilder Inject<T>(this IContainerBuilder builder) where T : Component
    {
        builder.RegisterBuildCallback(resolver =>
        {
            Debug.Log("Injected");
            var instance = resolver.Resolve<T>();
            resolver.Inject(instance);
        });

        return builder;
    }

    public static IContainerBuilder NonLazy<T>(this IContainerBuilder builder, T instance)
    {
        builder.RegisterBuildCallback(resolver => resolver.Resolve(instance.GetType()));
        return builder;
    }
}
