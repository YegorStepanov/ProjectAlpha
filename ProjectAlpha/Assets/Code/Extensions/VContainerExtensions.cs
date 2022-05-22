using System;
using System.Runtime.CompilerServices;
using Code.AddressableAssets;
using Code.VContainer;
using UnityEngine;
using UnityEngine.Assertions;
using VContainer;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace Code;

public static class VContainerExtensions
{
    public static GameObject InstantiateInScene<T>(this LifetimeScope scope, GameObject prefab, Address<T> asset,
        Transform under = null)
        where T : Object
    {
        return scope.InstantiateInScene(prefab, asset.Key, under);
    }

    //Instantiate in the scope scene instead of the active scene
    public static GameObject InstantiateInScene(this LifetimeScope scope, GameObject prefab,
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

        scope.Container?.InjectGameObject(instance);
        return instance;
    }

    public static T InstantiateInScene<T>(this LifetimeScope scope, T prefab,
        string name = null, Transform under = null) where T : Component
    {
        T instance;
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

        scope.Container?.Inject(instance); //????
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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RegistrationBuilder RegisterAsync<TImplement, T>(
        this IContainerBuilder builder,
        Address<T> address,
        Lifetime lifetime)
        where TImplement : class, IAsyncObject<T>
        where T : Object
    {
        AssertAsyncTypeAndTypeParameter(typeof(TImplement), typeof(T));

        return builder.Register<TImplement>(lifetime).WithParameter(address);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RegistrationBuilder RegisterAsync<TInterface, TImplement, T>(
        this IContainerBuilder builder,
        Address<T> address,
        Lifetime lifetime)
        where TInterface : class, IAsyncObject<T>
        where TImplement : class, IAsyncObject<T>, TInterface
        where T : Object
    {
        AssertAsyncTypeAndTypeParameter(typeof(TImplement), typeof(T));

        return builder.Register<TInterface, TImplement>(lifetime).WithParameter(address);
    }

    private static void AssertAsyncTypeAndTypeParameter(Type asyncType, Type typeParameter)
    {
        bool isAsyncComponent = asyncType.IsAssignableToGenericType(typeof(AsyncComponent<>));
        bool isAsyncAsset = asyncType.IsAssignableToGenericType(typeof(AsyncAsset<>));
        bool isAsyncGameObject = asyncType.IsAssignableTo(typeof(AsyncGameObject));

        bool isComponent = typeParameter.IsAssignableTo(typeof(Component));
        bool isGameObject = typeParameter.IsAssignableTo(typeof(GameObject));

        if (isAsyncComponent)
        {
            const string msg = "T must be a Component type to be used in AsyncComponent<T>";
            Assert.IsTrue(isComponent, msg);
            Assert.IsTrue(!isGameObject, msg);
        }

        if (isAsyncAsset)
        {
            Assert.IsTrue(!isComponent, "T cannot be a Component type to be used in AsyncAsset<T>");
            Assert.IsTrue(!isGameObject, "T cannot be a GameObject type to be used in AsyncAsset<T>");
        }

        if (isAsyncGameObject)
        {
            const string msg = "T must be a GameObject type to be used in AsyncGameObject";
            Assert.IsTrue(!isComponent, msg);
            Assert.IsTrue(isGameObject, msg);
        }
    }
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