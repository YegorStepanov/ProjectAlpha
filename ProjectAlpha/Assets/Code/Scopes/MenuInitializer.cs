using Code.Services;
using Zenject;

namespace Code.Scopes;

public sealed class MenuInitializer : IInitializable
{
    private readonly MenuMediator mediator;

    public MenuInitializer(MenuMediator mediator) =>
        this.mediator = mediator;

    public void Initialize() =>
        mediator.Open<MainMenu>();
}