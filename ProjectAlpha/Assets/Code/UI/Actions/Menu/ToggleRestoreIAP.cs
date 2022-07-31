using Code.Common;
using UnityEngine;

namespace Code.UI.Actions;

public sealed class ToggleRestoreIAP : MonoBehaviour
{
    private void Awake() =>
        gameObject.SetActive(PlatformInfo.IsIAPButtonSupported); //todo
}
