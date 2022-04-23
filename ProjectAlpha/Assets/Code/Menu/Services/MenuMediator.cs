using UnityEngine;
using Zenject;

namespace Code.Menu
{
    public sealed class MenuMediator : MonoBehaviour
    {
        [Inject] private UIManager uiManager;

        public void ShowShopPanel() =>
            uiManager.ShowShopPanel().Forget();

        public void HideShopPanel() =>
            uiManager.HideShopPanel();

        public void ShowHeroSelectorPanel() =>
            uiManager.ShowHeroSelectorPanel().Forget();

        public void HideHeroSelectorPanel() =>
            uiManager.HideHeroSelectorPanel();
    }
}