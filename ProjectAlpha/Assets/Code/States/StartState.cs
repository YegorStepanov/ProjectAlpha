using Code.Services;
using Code.Services.Game.UI;
using Cysharp.Threading.Tasks;

namespace Code.States;

public sealed class StartState : IState
{
    private readonly GameUIController _gameUIController;
    private readonly GameWorld _gameWorld;
    private readonly HeroSpawner _heroSpawner;
    private readonly PlatformSpawner _platformSpawner;
    private readonly GameStateResetter _gameStateResetter;
    private readonly GameStartEventAwaiter _gameStartEventAwaiter;

    public StartState(GameUIController gameUIController, GameWorld gameWorld, HeroSpawner heroSpawner, PlatformSpawner platformSpawner, GameStateResetter gameStateResetter, GameStartEventAwaiter gameStartEventAwaiter)
    {
        _gameUIController = gameUIController;
        _gameWorld = gameWorld;
        _heroSpawner = heroSpawner;
        _platformSpawner = platformSpawner;
        _gameStateResetter = gameStateResetter;
        _gameStartEventAwaiter = gameStartEventAwaiter;
    }

    public async UniTaskVoid EnterAsync(IGameStateMachine stateMachine)
    {
        HideUI();
        SwitchToMenuHeight();
        await ResetGameState();

        IPlatform menuPlatform = await CreateMenuPlatform();
        IHero hero = CreateHero(menuPlatform);

        await WaitForGameStartEvent();
        ShowUI();

        stateMachine.Enter<MoveHeroToMenuPlatformState, MoveHeroToMenuPlatformState.Arguments>(
            new(hero, menuPlatform));
    }

    private void SwitchToMenuHeight() =>
        _gameWorld.SwitchToMenuHeight();

    private UniTask ResetGameState() =>
        _gameStateResetter.ResetAsync();

    private UniTask<IPlatform> CreateMenuPlatform() =>
        _platformSpawner.CreateMenuPlatformAsync();

    private IHero CreateHero(IPlatform menuPlatform) =>
        _heroSpawner.Create(menuPlatform.Borders.CenterTop, Relative.Left);

    private UniTask WaitForGameStartEvent() =>
        _gameStartEventAwaiter.Wait();

    private void HideUI() =>
        _gameUIController.HideUI();

    private void ShowUI() =>
        _gameUIController.ShowUI();
}
