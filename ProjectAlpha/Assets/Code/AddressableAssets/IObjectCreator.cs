using UnityEngine;

namespace Code.AddressableAssets
{
    public interface IObjectCreator
    {
        GameObject InstantiateEmpty(string name);

        T Instantiate<T>(T prefab) where T : Object;
        T Instantiate<T>(T prefab, Vector3 position, Quaternion rotation) where T : Object;
        T Instantiate<T>(T prefab, Vector3 position, Quaternion rotation, Transform parent) where T : Object;
        T Instantiate<T>(T prefab, Transform parent, bool worldPositionStays = false) where T : Object;

        T InstantiateNoInject<T>(T prefab) where T : Object;
        T InstantiateNoInject<T>(T prefab, Vector3 position, Quaternion rotation) where T : Object;
        T InstantiateNoInject<T>(T prefab, Vector3 position, Quaternion rotation, Transform parent) where T : Object;
        T InstantiateNoInject<T>(T prefab, Transform parent, bool worldPositionStays = false) where T : Object;
    }
}
