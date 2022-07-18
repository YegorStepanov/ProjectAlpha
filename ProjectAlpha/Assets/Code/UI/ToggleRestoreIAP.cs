using Code.Infrastructure;
using UnityEngine;

namespace Code.UI;

public sealed class ToggleRestoreIAP : MonoBehaviour
{
    [SerializeField] private RectTransform _layout;

    private void Awake()
    {
        gameObject.SetActive(PlatformInfo.IsIAPButtonSupported);
    }
}
