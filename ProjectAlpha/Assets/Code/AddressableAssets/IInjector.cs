using UnityEngine;

namespace Code.AddressableAssets;

public interface IInjector
{
    void Inject(object instance);
    void InjectGameObject(GameObject gameObject);
}
