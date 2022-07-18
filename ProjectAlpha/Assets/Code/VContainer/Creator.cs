using UnityEngine;
using VContainer.Unity;

namespace Code;

public sealed class Creator : ICreator
{
    private readonly LifetimeScope _scope;

    public Creator(LifetimeScope scope) =>
        _scope = scope;

    public GameObject Instantiate(string name)
    {
        if (_scope.IsRoot)
        {
            GameObject gameObject = new(name);
            Object.DontDestroyOnLoad(gameObject);
            return gameObject;
        }

        //mb we can manipulate the temp?
        GameObject temp = new();
        GameObject gameObjectInScopeScene = Object.Instantiate(temp, _scope.transform);

        Transform t = gameObjectInScopeScene.transform;
        t.SetParent(null);
        t.name = name;

        Object.Destroy(temp);
        return gameObjectInScopeScene;
    }

    public GameObject Instantiate(GameObject prefab, bool inject)
    {
        GameObject instance = InstantiateInScopeScene(prefab);
        instance.name = prefab.name;

        if (inject)
            Inject(instance);

        return instance;
    }

    private GameObject InstantiateInScopeScene(GameObject prefab)
    {
        if (_scope.IsRoot)
        {
            GameObject instance = Object.Instantiate(prefab);
            Object.DontDestroyOnLoad(instance);
            return instance;
        }
        else
        {
            GameObject instance = Object.Instantiate(prefab, _scope.transform);
            instance.transform.SetParent(null);
            return instance;
        }
    }

    private void Inject(GameObject instance)
    {
        if (_scope.Container == null)
            Debug.LogError("scope.Container is null");
        else
            _scope.Container.InjectGameObject(instance);
    }
}
