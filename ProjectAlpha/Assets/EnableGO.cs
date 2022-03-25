using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableGO : MonoBehaviour
{
    [SerializeField] private GameObject go;

    public void Enable() =>
        go.SetActive(true);
}
