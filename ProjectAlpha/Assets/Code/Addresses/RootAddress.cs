using Code.AddressableAssets;
using Code.Services.Infrastructure;
using Code.Settings;
using UnityEngine.EventSystems;

namespace Code.Addresses;

public static class RootAddress
{
    public static readonly Address<Camera1> CameraController = new("Camera");
    public static readonly Address<SettingsFacade> Settings = new("Settings");
    public static readonly Address<EventSystem> EventSystem = new("EventSystem");
}
