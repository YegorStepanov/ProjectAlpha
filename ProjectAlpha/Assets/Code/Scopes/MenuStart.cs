using Code.Services;
using VContainer.Unity;

namespace Code.Scopes;

public sealed class MenuStart : IStartable
{
    private readonly MenuMediator _mediator;

    public MenuStart(MenuMediator mediator) =>
        _mediator = mediator;

    public void Start() =>
        _mediator.Open<MainMenu>();
}