using UnityEngine;

namespace Code.AddressableAssets;

public abstract class BehaviourFactory<TValue> : IFactory<TValue> where TValue : Object
{
    private readonly IObjectCreator _creator;
    private readonly TValue _prefab;
    private readonly string _name;
    private readonly string _containerName;

    private Transform _container;

    protected BehaviourFactory(IObjectCreator creator, TValue prefab, string name, string parentName)
    {
        _creator = creator;
        _prefab = prefab;
        _name = name;
        _containerName = parentName;
    }

    public TValue Create()
    {
        if (_container == null)
            _container = _creator.InstantiateEmpty(_containerName).transform;

        TValue instance = _creator.Instantiate(_prefab, _container);
        instance.name = _name;
        return instance;
    }
}
