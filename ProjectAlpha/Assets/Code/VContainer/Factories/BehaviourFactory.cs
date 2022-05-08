using UnityEngine;
using VContainer.Unity;

namespace Code.VContainer;

public abstract class BehaviourFactory<TValue> : IFactory<TValue> where TValue : Object
{
    private readonly TValue _prefab;
    private readonly string _name;
    private readonly string _parentName;
    private readonly LifetimeScope _scope;

    private Transform _parentInstance;

    protected BehaviourFactory(TValue prefab, InstanceName name, ParentName parentName, LifetimeScope scope)
    {
        _prefab = prefab;
        _name = name.Name;
        _parentName = parentName.Name;
        _scope = scope;
    }

    public TValue Create()
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

        //_scope.transform == null when the container is rooted
        _parentInstance = Object.Instantiate(temp, _scope.transform).transform;
        _parentInstance.SetParent(null);
        _parentInstance.name = _parentName;

        Object.Destroy(temp);
    }
}