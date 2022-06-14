using UnityEngine;

namespace Code.UI;

public sealed class ToggleRestoreIAP : MonoBehaviour
{
    [SerializeField] private RectTransform _layout;

    private void Awake()
    {
#if UNITY_EDITOR || UNITY_IOS || UNITY_STANDALONE_OSX || UNITY_WSA
        gameObject.SetActive(true);
#else
        gameObject.SetActive(false);
#endif
    }
}