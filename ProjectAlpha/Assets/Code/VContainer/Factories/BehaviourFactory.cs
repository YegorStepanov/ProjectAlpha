using UnityEngine;
using VContainer.Unity;

namespace Code.VContainer;

public abstract class BehaviourFactory<TValue> : IFactory<TValue> where TValue : Object
{
    private readonly TValue _prefab;
    private readonly string _name;
    private readonly string _containerName;
    private readonly LifetimeScope _scope;

    private Transform _container;

    protected BehaviourFactory(TValue prefab, InstanceName name, ParentName parentName, LifetimeScope scope)
    {
        _prefab = prefab;
        _name = name.Name;
        _containerName = parentName.Name;
        _scope = scope;
    }

    public TValue Create()
    {
        _container ??= _scope.CreateRootSceneContainer(_containerName);

        TValue instance = Object.Instantiate(_prefab, _container);
        instance.name = _name;

        if (instance is GameObject go)
            _scope.Container.InjectGameObject(go);
        else
            _scope.Container.Inject(instance);

        return instance;
    }
}