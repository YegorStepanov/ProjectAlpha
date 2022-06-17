using System.Threading;
using Code.AddressableAssets;
using Code.Services;
using Code.Services.Monetization;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Code.Scopes;

public sealed class MenuScope : Scope
{
    private MenuMediator _menu;
    private MainMenu _mainMenu;

    protected override async UniTask PreloadAsync(IAddressablesLoader loader)
    {
        var loadMediator = loader.LoadAssetAsync(MenuAddress.MenuMediator);
        var loadMainMenu = loader.LoadAssetAsync(MenuAddress.MainMenu);

        (_menu, _mainMenu) = await (loadMediator, loadMainMenu);
    }

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponentInNewPrefab(_menu, Lifetime.Singleton);
        builder.RegisterComponentInNewPrefab(_mainMenu, Lifetime.Singleton);

        builder.Register<PanelManager>(Lifetime.Singleton);

        builder.RegisterInstance(this.GetCancellationTokenOnDestroy());

        builder.RegisterBuildCallback(BuildCallback);
        builder.RegisterEntryPoint<MenuEntryPoint>();
    }

    private static void BuildCallback(IObjectResolver resolver)
    {
        var mainMenu = resolver.Resolve<MainMenu>();
        resolver.InjectGameObject(mainMenu.gameObject);
    }
}

public class MenuEntryPoint : IAsyncStartable
{
    private readonly Ads _ads;

    public MenuEntryPoint(Ads ads)
    {
        _ads = ads;
    }

    public async UniTask StartAsync(CancellationToken token)
    {
        return;
        await _ads.ShowBannerAsync(token);
        await UniTask.Delay(1000, cancellationToken: token);
        await _ads.ShowInterstitialAsync(token);
        Debug.Log("Frame after show: " + Time.frameCount);
        await UniTask.Delay(3000, cancellationToken: token);
        Debug.Log("show rewarded: " + Time.frameCount);
        await _ads.ShowRewardedAsync(token);
        Debug.Log("showed rewarded: " + Time.frameCount);
    }
}