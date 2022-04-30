using Code.Services;
using UnityEngine;

namespace Code.Scopes;

public sealed class MenuInstaller : BaseInstaller<MenuInitializer>
{
    [SerializeField] private MenuMediator menu;

    public override void InstallBindings()
    {
        base.InstallBindings();

        // Container.Bind<StartGameTrigger>().AsSingle(); ??

        RegisterMenuMediator();

        RegisterUIManager();
    }

    private void RegisterMenuMediator() =>
        Container.BindInstance(menu);

    private void RegisterUIManager() =>
        Container.Bind<UIManager>().AsSingle();
}