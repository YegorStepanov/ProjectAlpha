using System.Threading;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;

namespace Code.Services;

public sealed class MenuMediator : MonoBehaviour
{
    private MainMenu _mainMenu;
    private PanelManager _panelManager;
    private ISceneLoader _sceneLoader;
    private CancellationToken _token;

    [Inject, UsedImplicitly]
    public void Construct(
        MainMenu mainMenu,
        PanelManager panelManager,
        ISceneLoader sceneLoader,
        CancellationToken token)
    {
        _mainMenu = mainMenu;
        _panelManager = panelManager;
        _sceneLoader = sceneLoader;
        _token = token;
    }

    public void Open<TPanel>() where TPanel : struct, IPanel => _panelManager.Show<TPanel>();
    public void Close<TPanel>() where TPanel : struct, IPanel => _panelManager.Hide<TPanel>();

    [Button] public void CloseScene() => _sceneLoader.UnloadAsync<MenuScene>(_token);
    [Button] public void ToggleSound() => _mainMenu.ToggleSound();

#if UNITY_EDITOR
    //Odin doesn't recognize generic methods, so resolve them manually
    [BoxGroup("Shop Panel")]
    [HorizontalGroup("Shop Panel/b"), Button("Show")]
    private void ShowShopPanel() => _panelManager.Show<ShopPanel>();

    [HorizontalGroup("Shop Panel/b"), Button("Hide")]
    private void HideShopPanel() => _panelManager.Hide<ShopPanel>();

    [HorizontalGroup("Shop Panel/b"), Button("Unload")]
    private void UnloadShopPanel() => _panelManager.Unload<ShopPanel>();

    [BoxGroup("Hero Selector Panel")]
    [HorizontalGroup("Hero Selector Panel/b"), Button("Show")]
    private void ShowHeroSelectorPanel() => _panelManager.Show<HeroSelectorPanel>();

    [HorizontalGroup("Hero Selector Panel/b"), Button("Hide")]
    private void HideHeroSelectorPanel() => _panelManager.Hide<HeroSelectorPanel>();

    [HorizontalGroup("Hero Selector Panel/b"), Button("Unload")]
    private void UnloadHeroSelectorPanel() => _panelManager.Unload<HeroSelectorPanel>();
#endif
}