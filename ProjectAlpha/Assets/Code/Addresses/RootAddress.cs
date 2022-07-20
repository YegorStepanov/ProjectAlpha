using Code.AddressableAssets;
using Code.Game;
using Code.Services;
using UnityEngine.EventSystems;

namespace Code;

public static class RootAddress
{
    public static readonly Address<Camera> CameraController = new("Camera");
    public static readonly Address<SettingsFacade> Settings = new("Settings");
    public static readonly Address<EventSystem> EventSystem = new("EventSystem");
}
