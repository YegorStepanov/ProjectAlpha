using Code.Services;
using UnityEngine;

namespace Code.Scopes;

public sealed class MenuInstaller : BaseInstaller<MenuInitializer>
{
    [SerializeField] private MenuMediator _menu;

    public override void InstallBindings()
    {
        base.InstallBindings();

        RegisterMenuMediator();

        RegisterUIManager();
    }

    private void RegisterMenuMediator() =>
        Container.BindInstance(_menu);

    private void RegisterUIManager() =>
        Container.Bind<UIManager>().AsSingle();
}