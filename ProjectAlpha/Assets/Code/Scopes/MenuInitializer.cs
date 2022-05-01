using Code.Services;
using Zenject;

namespace Code.Scopes;

public sealed class MenuInitializer : IInitializable
{
    private readonly MenuMediator _mediator;

    public MenuInitializer(MenuMediator mediator) =>
        _mediator = mediator;

    public void Initialize() =>
        _mediator.Open<MainMenu>();
}