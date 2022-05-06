using Code.Services;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Code.Scopes;

public sealed class MenuScope : LifetimeScope
{
    [SerializeField] private MenuMediator _menu;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponent(_menu);
        builder.Register<UIManager>(Lifetime.Singleton);

        builder.RegisterEntryPoint<MenuStart>();
        
        builder.Register<AddressableFactory>(Lifetime.Singleton);

        builder.RegisterInstance(this.GetCancellationTokenOnDestroy());
    }
}