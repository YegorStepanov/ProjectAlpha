using UnityEngine;

namespace Code.AddressableAssets;

public interface ICreator
{
    GameObject Instantiate(string name);
    GameObject Instantiate(GameObject prefab, bool inject);
}
