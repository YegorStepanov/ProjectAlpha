using UnityEngine;

namespace Code;

public interface ICreator
{
    GameObject Instantiate(string name);
    GameObject Instantiate(GameObject prefab, bool inject);
}
