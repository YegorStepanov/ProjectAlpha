using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Code.Menu
{
    public class UIManager
    {
        private readonly DiContainer container;

        private GameObject shopPanel;
        private GameObject heroSelectorPanel;
        
        public UIManager(DiContainer container) =>
            this.container = container;

        public async UniTaskVoid ShowShopPanel() =>
            shopPanel = await ShowPanelAsync(shopPanel, UIAssetAddress.ShopPanel);

        public async UniTaskVoid ShowHeroSelectorPanel() =>
            heroSelectorPanel = await ShowPanelAsync(heroSelectorPanel, UIAssetAddress.HeroSelectorPanel);

        public void HideShopPanel() =>
            HidePanel(shopPanel);

        public void HideHeroSelectorPanel() =>
            HidePanel(heroSelectorPanel);

        private async UniTask<GameObject> ShowPanelAsync(GameObject panel, string selectorPanel)
        {
            if (panel == null)
            {
                panel = await Addressables.InstantiateAsync(selectorPanel);
                container.InjectGameObject(panel);
            }

            panel.SetActive(true);
            return panel;
        }

        private static void HidePanel(GameObject panel) =>
            panel.SetActive(false);
    }
}