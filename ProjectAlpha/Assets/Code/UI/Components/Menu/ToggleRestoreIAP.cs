using Code.Common;
using UnityEngine;

namespace Code.UI.Components;

public sealed class ToggleRestoreIAP : MonoBehaviour
{
    private void Awake() =>
        gameObject.SetActive(PlatformInfo.IsIAPButtonSupported); //todo
}
