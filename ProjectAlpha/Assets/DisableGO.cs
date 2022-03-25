using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableGO : MonoBehaviour
{
    [SerializeField] private GameObject go;
    
    public void Disable() => 
        go.SetActive(false);
}
