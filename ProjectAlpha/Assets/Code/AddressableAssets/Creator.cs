using UnityEngine;
using VContainer.Unity;

namespace Code.AddressableAssets;

public sealed class Creator : ICreator
{
    private readonly LifetimeScope _scope;
    private readonly GameObject _emptyGameObject = new();

    public Creator(LifetimeScope scope) =>
        _scope = scope;

    public GameObject Instantiate(string name) =>
        _scope.IsRoot
            ? CreateInRootScene(_emptyGameObject, name)
            : CreateInScopeScene(_emptyGameObject, name);

    public GameObject Instantiate(GameObject prefab) =>
        _scope.IsRoot
            ? CreateInRootScene(prefab, prefab.name)
            : CreateInScopeScene(prefab, prefab.name);

    private static GameObject CreateInRootScene(GameObject prefab, string name)
    {
        GameObject instance = Object.Instantiate(prefab);
        Object.DontDestroyOnLoad(instance);
        instance.name = name;
        return instance;
    }

    private GameObject CreateInScopeScene(GameObject prefab, string name)
    {
        GameObject instance = Object.Instantiate(prefab, _scope.transform);
        instance.transform.SetParent(null);
        instance.name = name;
        return instance;
    }
}
