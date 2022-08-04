using Code.AddressableAssets;
using Code.Animations.Game;
using Code.Data;
using Code.Services.Development;
using Code.Services.Entities;
using Code.Services.Infrastructure;
using Code.Services.UI;
using Code.UI;
using Tayx.Graphy;
using UnityEngine;
using UnityEngine.EventSystems;
using SceneType = Code.Common.Scene;
using HeroComponent = Code.Services.Entities.Hero;

namespace Code;

public static class Address
{
    public static class Scene
    {
        //note: scene name and address must match
        public static readonly Address<SceneType> Bootstrap = new("Bootstrap");
        public static readonly Address<SceneType> Menu = new("Menu");
        public static readonly Address<SceneType> Game = new("Game");
    }

    public static class Infrastructure
    {
        public static readonly Address<Camera1> CameraController = new("Camera");
        public static readonly Address<SettingsFacade> Settings = new("Settings Facade");
        public static readonly Address<EventSystem> EventSystem = new("EventSystem");
    }

    public static class Data
    {
        public static readonly Address<PlatformWidthGeneratorData> WidthGenerator = new("Width Generator");
        public static readonly Address<PlatformPositionGenerator> PlatformPositionGenerator = new("Platform Position Generator");
        public static readonly Address<CherryPositionGenerator> CherryPositionGenerator = new("Cherry Position Generator");
    }

    public static class UI
    {
        public static readonly Address<GameUIView> GameUI = new("Game UI");
        public static readonly Address<RedPointHitGameAnimation> RedPointHitAnimation = new("Plus 1 Notification");

        public static readonly Address<GameObject> ShopPanel = new("Shop Panel");
        public static readonly Address<GameObject> HeroSelectorPanel = new("Hero Selector Panel");

        public static readonly Address<MenuUIFacade> MenuUIActions = new("Menu Mediator");
        public static readonly Address<MainMenuView> MainMenuView = new("Main Menu");

        public static readonly Address<LoadingScreen> TransitionLoadingScreen = new("Transition Loading Screen");
    }

    public static class Entity
    {
        public static class Hero
        {
            public static readonly Address<HeroComponent> Hero1 = new("Hero1");
            public static readonly Address<HeroComponent> Hero2 = new("Hero2");
            public static readonly Address<HeroComponent> Hero3 = new("Hero3");
            public static readonly Address<HeroComponent> Hero4 = new("Hero4");
        }

        public static readonly Address<Stick> Stick = new("Stick");
        public static readonly Address<Platform> Platform = new("Platform");
        public static readonly Address<Cherry> Cherry = new("Cherry");
    }

    public static class Background
    {
        public static readonly Address<Texture2D> Background1 = new("Background 1");
        public static readonly Address<Texture2D> Background2 = new("Background 2");
        public static readonly Address<Texture2D> Background3 = new("Background 3");
        public static readonly Address<Texture2D> Background4 = new("Background 4");
        public static readonly Address<Texture2D> Background5 = new("Background 5");
    }

    public static class Development
    {
        public static readonly Address<GraphyManager> Graphy = new("Graphy");
        public static readonly Address<DevelopmentPanel> Panel = new("Development Panel");
    }
}
