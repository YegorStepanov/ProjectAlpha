﻿using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Services;

public sealed class UIManager
{
    private readonly GlobalAddressableLoader _loader;

    private GameObject _mainMenu;
    private GameObject _shopPanel;
    private GameObject _heroSelectorPanel;

    public UIManager(GlobalAddressableLoader loader) =>
        _loader = loader;

    public void Show<TPanel>() where TPanel : struct, IPanel
    {
        ShowAsync().Forget();

        async UniTaskVoid ShowAsync()
        {
            GameObject panel = GetPanel<TPanel>();
            Address<GameObject> address = GetAddress<TPanel>();
            SetPanel<TPanel>(await GetOrCreatePanelAsync(panel, address));
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
        _loader.Release(panel);
    }

    private async UniTask<GameObject> GetOrCreatePanelAsync(GameObject panel, Address<GameObject> address)
    {
        if (panel == null)
            panel = await _loader.LoadAsync(address);

        panel.SetActive(true);
        return panel;
    }

    private void SetPanel<TPanel>(GameObject panel) where TPanel : struct, IPanel
    {
        switch (typeof(TPanel))
        {
            case Type t when t == typeof(MainMenu):
                _mainMenu = panel;
                break;
            case Type t when t == typeof(ShopPanel):
                _shopPanel = panel;
                break;
            case Type t when t == typeof(HeroSelectorPanel):
                _heroSelectorPanel = panel;
                break;
            default:
                throw new ArgumentOutOfRangeException(typeof(TPanel).FullName);
        }
    }

    private GameObject GetPanel<TPanel>() where TPanel : struct, IPanel => typeof(TPanel) switch
    {
        Type t when t == typeof(MainMenu) => _mainMenu,
        Type t when t == typeof(ShopPanel) => _shopPanel,
        Type t when t == typeof(HeroSelectorPanel) => _heroSelectorPanel,
        _ => throw new ArgumentOutOfRangeException(typeof(TPanel).FullName)
    };

    private static Address<GameObject> GetAddress<TPanel>() where TPanel : struct, IPanel => typeof(TPanel) switch
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