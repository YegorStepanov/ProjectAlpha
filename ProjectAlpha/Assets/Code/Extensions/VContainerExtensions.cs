﻿using System.Runtime.CompilerServices;
using Code.AddressableAssets;
using Code.VContainer;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Code;

public static class VContainerExtensions
{
    public static GameObject Instantiate<T>(this LifetimeScope scope, GameObject prefab, Address<T> asset,
        Transform under = null)
        where T : Object
    {
        return scope.Instantiate(prefab, asset.Key, under);
    }

    //Instantiate in the scope scene instead of the active scene
    public static GameObject Instantiate(this LifetimeScope scope, GameObject prefab,
        string name = null, Transform under = null)
    {
        GameObject instance;
        if (under != null)
        {
            instance = Object.Instantiate(prefab, under);
        }
        else if (scope.IsRoot)
        {
            instance = Object.Instantiate(prefab);
            Object.DontDestroyOnLoad(instance);
        }
        else
        {
            instance = Object.Instantiate(prefab, scope.transform);
            instance.transform.SetParent(null);
        }

        if (name is not null)
            instance.name = name;

        scope.Container.InjectGameObject(instance);
        return instance;
    }

    public static Transform CreateRootSceneContainer(this LifetimeScope scope, string name)
    {
        if (scope.IsRoot)
        {
            GameObject go = new(name);
            //instantiate?
            Object.DontDestroyOnLoad(go);
            return go.transform;
        }

        GameObject temp = new();

        Transform sceneContainer = Object.Instantiate(temp, scope.transform).transform;
        sceneContainer.SetParent(null);
        sceneContainer.name = name;

        Object.Destroy(temp);
        return sceneContainer;
    }

    // public static void InstantiateInScene<TObject>(this LifetimeScope scope, TObject instance) where TObject : Object
    // {
    //     // TObject instance;
    //
    //     if (instance is GameObject go)
    //         scope.InstantiateInScene(go);
    //     
    //     var a = Object.Instantiate(instance);
    //     scope.Container.Inject(a);
    //     
    //     if (scope.IsRoot)
    //     {
    //         Object.DontDestroyOnLoad(instance);
    //     }
    //     else
    //     {
    //         instance = Object.Instantiate(prefab, scope.transform);
    //         instance.transform.SetParent(null);
    //     }
    //     
    //     scope.Container.InjectGameObject(instance);
    //     return instance;
    // }

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
    public static RegistrationBuilder RegisterAddressablePool<T>(
        this IContainerBuilder builder,
        Address<T> address,
        string containerName,
        int initialSize,
        int capacity,
        Lifetime lifetime)
        where T : Component
    {
        var data = new ComponentPoolData(containerName, initialSize, capacity);
        data.Validate();

        return builder.Register<AddressableComponentPool<T>>(lifetime)
            .WithParameter(address)
            .WithParameter(data);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RegistrationBuilder RegisterAddressableAssetPool<T>(
        this IContainerBuilder builder,
        Address<T> address,
        int initialSize,
        int capacity,
        Lifetime lifetime)
        where T : Object
    {
        var data = new PoolData(initialSize, capacity);
        data.Validate();

        return builder.Register<AddressableAssetPool<T>>(lifetime)
            .WithParameter(address)
            .WithParameter(data);
    }
    
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

public record PoolData(int InitialSize, int Capacity)
{
    public void Validate()
    {
        if (InitialSize < 0)
            Debug.LogError("Wrong initialSize:" + InitialSize);

        if (Capacity < 0)
            Debug.LogError("Wrong capacity:" + Capacity);
    }
}

public sealed record ComponentPoolData(string ContainerName, int InitialSize, int Capacity) :
    PoolData(InitialSize, Capacity);