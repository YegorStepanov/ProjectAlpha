using UnityEngine;
using VContainer.Unity;

namespace Code.VContainer;

public abstract class BehaviourPool<TValue> : Pool<TValue> where TValue : Object
{
    private readonly TValue _prefab;
    private readonly string _name;
    private readonly string _parentName;
    private readonly LifetimeScope _scope;

    private Transform _parentInstance;

    protected BehaviourPool(TValue prefab, InstanceName name, ParentName parentName, InitialSize initialSize, Capacity capacity, LifetimeScope scope)
        : base(initialSize, capacity)
    {
        _prefab = prefab;
        _name = name.Name;
        _parentName = parentName.Name;
        _scope = scope;
    }

    protected override TValue Create()
    {
        if (_parentInstance == null)
            CreateParent();

        TValue instance = Object.Instantiate(_prefab, _parentInstance);
        instance.name = _name;

        if (instance is GameObject go)
            _scope.Container.InjectGameObject(go);
        else
            _scope.Container.Inject(instance);

        return instance;
    }

    private void CreateParent()
    {
        var temp = new GameObject(_parentName);

        //todo: _scope.transform == null when the container is rooted
        _parentInstance = Object.Instantiate(temp, _scope.transform).transform;
        _parentInstance.SetParent(null);
        _parentInstance.name = _parentName;

        Object.Destroy(temp);
    }
}