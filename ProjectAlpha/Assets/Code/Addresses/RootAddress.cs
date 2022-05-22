using Code.AddressableAssets;
using Code.Game;
using Code.Services;
using Tayx.Graphy;

namespace Code;

public static class RootAddress
{
    public static readonly Address<CameraController> CameraController = new("Camera");
    public static readonly Address<GameSettings> Settings = new("Settings");
    public static readonly Address<GraphyManager> Graphy = new("Graphy");
}