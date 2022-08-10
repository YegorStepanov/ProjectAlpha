using UnityEngine;
using VContainer.Unity;

namespace Code.AddressableAssets;

public sealed class Injector : IInjector
{
    private readonly LifetimeScope _root;

    public Injector(LifetimeScope root) =>
        _root = root;

    public void Inject(object instance) =>
        _root.Container.Inject(instance);

    public void InjectGameObject(GameObject gameObject) =>
        _root.Container.InjectGameObject(gameObject);
}
