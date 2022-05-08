using System.Runtime.CompilerServices;
using Code.VContainer;
using UnityEngine;
using VContainer;

namespace Code;

public static class VContainerExtensions
{
    public static T ResolveInstance<T>(this IObjectResolver resolver)
    {
        var registrationBuilder = new RegistrationBuilder(typeof(T), Lifetime.Transient);

        Registration registration = registrationBuilder.Build();
        return (T)resolver.Resolve(registration);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RegistrationBuilder RegisterMonoBehaviourFactory<T>(
        this IContainerBuilder builder,
        T prefab,
        string name,
        string containerName,
        Lifetime lifetime) where T : MonoBehaviour
        => builder.Register<MonoBehaviourFactory<T>>(lifetime)
            .WithParameter(prefab)
            .WithParameter(new InstanceName(name))
            .WithParameter(new ParentName(containerName));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RegistrationBuilder RegisterScriptableObjectFactory<T>(
        this IContainerBuilder builder,
        T prefab,
        string name,
        string containerName,
        Lifetime lifetime) where T : ScriptableObject
        => builder.Register<ScriptableObjectFactory<T>>(lifetime)
            .WithParameter(prefab)
            .WithParameter(new InstanceName(name))
            .WithParameter(new ParentName(containerName));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RegistrationBuilder RegisterMonoBehaviourPool<T>(
        this IContainerBuilder builder,
        T prefab,
        string name,
        string containerName,
        int initialSize,
        int size,
        Lifetime lifetime) where T : MonoBehaviour
        => builder.Register<MonoBehaviourPool<T>>(lifetime)
            .WithParameter(prefab)
            .WithParameter(new InstanceName(name))
            .WithParameter(new ParentName(containerName))
            .WithParameter(new InitialSize(initialSize))
            .WithParameter(new Capacity(size));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RegistrationBuilder RegisterScriptableObjectPool<T>(
        this IContainerBuilder builder,
        T prefab,
        string name,
        string containerName,
        int initialSize,
        int size,
        Lifetime lifetime) where T : ScriptableObject
        => builder.Register<ScriptableObjectPool<T>>(lifetime)
            .WithParameter(prefab)
            .WithParameter(new InstanceName(name))
            .WithParameter(new ParentName(containerName))
            .WithParameter(new InitialSize(initialSize))
            .WithParameter(new Capacity(size));
}