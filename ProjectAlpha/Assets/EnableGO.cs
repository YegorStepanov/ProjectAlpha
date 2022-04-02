using UnityEngine;

public class EnableGO : MonoBehaviour
{
    [SerializeField] private GameObject go;

    public void Enable() =>
        go.SetActive(true);
}
