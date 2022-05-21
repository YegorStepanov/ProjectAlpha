using Code.AddressableAssets;
using UnityEngine;

namespace Code;

public static class SceneAddress
{
    //Scene name and address must match
    public static readonly Address<Scene> Bootstrap = new("Bootstrap");
    public static readonly Address<Scene> Menu = new("Menu");
    public static readonly Address<Scene> Game = new("Game");
    public static readonly Address<Scene> MiniGame = new("MiniGame");
    
    public static readonly Address<Scene> Bootstrapper = new("Bootstrap1");
}

public sealed class Scene : Object { }