using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Services
{
    public sealed class UIManager
    {
        private readonly AddressableFactory factory;

        private GameObject mainMenu;
        private GameObject shopPanel;
        private GameObject heroSelectorPanel;

        public UIManager(AddressableFactory factory) =>
            this.factory = factory;

        public void Show<TPanel>() where TPanel : struct, IPanel
        {
            Core().Forget();

            async UniTaskVoid Core()
            {
                GameObject panel = GetPanel<TPanel>();
                Address address = GetAddress<TPanel>();
                SetPanel<TPanel>(await GetOrCreateEnabledPanelAsync(panel, address));
            }
        }

        public void Hide<TPanel>() where TPanel : struct, IPanel
        {
            GameObject panel = GetPanel<TPanel>();
            panel.SetActive(false);
        }

        public void Unload<TPanel>() where TPanel : struct, IPanel
        {
            GameObject panel = GetPanel<TPanel>();
            factory.ReleaseInstance(panel);
        }

        private async UniTask<GameObject> GetOrCreateEnabledPanelAsync(GameObject panel, Address address)
        {
            if (panel == null)
                panel = await factory.InstantiateAsync(address);

            panel.SetActive(true);
            return panel;
        }

        private void SetPanel<TPanel>(GameObject panel) where TPanel : struct, IPanel
        {
            switch (typeof(TPanel))
            {
                case Type t when t == typeof(MainMenu):
                    mainMenu = panel;
                    break;
                case Type t when t == typeof(ShopPanel):
                    shopPanel = panel;
                    break;
                case Type t when t == typeof(HeroSelectorPanel):
                    heroSelectorPanel = panel;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(typeof(TPanel).FullName);
            }
        }

        private GameObject GetPanel<TPanel>() where TPanel : struct, IPanel => typeof(TPanel) switch
        {
            Type t when t == typeof(MainMenu) => mainMenu,
            Type t when t == typeof(ShopPanel) => shopPanel,
            Type t when t == typeof(HeroSelectorPanel) => heroSelectorPanel,
            _ => throw new ArgumentOutOfRangeException(typeof(TPanel).FullName)
        };

        private static Address GetAddress<TPanel>() where TPanel : struct, IPanel => typeof(TPanel) switch
        {
            Type t when t == typeof(MainMenu) => MenuAddress.MainMenu,
            Type t when t == typeof(ShopPanel) => MenuAddress.ShopPanel,
            Type t when t == typeof(HeroSelectorPanel) => MenuAddress.HeroSelectorPanel,
            _ => throw new ArgumentOutOfRangeException(typeof(TPanel).FullName)
        };
    }

    public interface IPanel { }

    public struct MainMenu : IPanel { }

    public struct ShopPanel : IPanel { }

    public struct HeroSelectorPanel : IPanel { }
}