using Code.Infrastructure;
using UnityEngine;

namespace Code.UI;

public sealed class ToggleRestoreIAP : MonoBehaviour
{
    private void Awake()
    {
        gameObject.SetActive(PlatformInfo.IsIAPButtonSupported);
    }
}
