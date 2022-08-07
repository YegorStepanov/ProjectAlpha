﻿using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Code.AddressableAssets;

public abstract class BehaviourFactory<TValue> : IFactory<TValue> where TValue : Object
{
    private readonly ICreator _creator;
    private readonly IObjectResolver _resolver;
    private readonly TValue _prefab;
    private readonly string _name;
    private readonly string _containerName;

    private Transform _container;

    protected BehaviourFactory(ICreator creator, IObjectResolver resolver, TValue prefab, string name, string parentName)
    {
        _creator = creator;
        _resolver = resolver;
        _prefab = prefab;
        _name = name;
        _containerName = parentName;
    }

    public TValue Create()
    {
        if (_container == null)
            _container = _creator.Instantiate(_containerName).transform;

        TValue instance = Object.Instantiate(_prefab, _container);
        instance.name = _name;

        if (instance is GameObject gameObject)
            _resolver.InjectGameObject(gameObject);
        else
            _resolver.Inject(instance);

        return instance;
    }
}