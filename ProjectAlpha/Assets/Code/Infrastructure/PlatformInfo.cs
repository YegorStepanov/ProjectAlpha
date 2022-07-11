using UnityEngine;

namespace Code.Infrastructure;

public static class PlatformInfo
{
    public static bool IsIAPButtonSupported => IsEditor || Application.platform
        is RuntimePlatform.IPhonePlayer
        or RuntimePlatform.OSXPlayer
        or RuntimePlatform.WSAPlayerX86
        or RuntimePlatform.WSAPlayerX64
        or RuntimePlatform.WSAPlayerARM;

    public static bool IsEditor => Application.isEditor;
    public static bool IsPlayer => !IsEditor;

    public static bool IsApple => Application.platform
        is RuntimePlatform.IPhonePlayer
        or RuntimePlatform.OSXPlayer;

    public static bool IsDevelopment => Debug.isDebugBuild;
}