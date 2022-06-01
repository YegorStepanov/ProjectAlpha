using System.Threading;
using Code.States;
using Cysharp.Threading.Tasks;

namespace Code.Services.Game.UI;

public class GameSceneLoader
{
    private readonly ISceneLoader _sceneLoader;
    private readonly GameUIController _gameUIController;
    private readonly CancellationToken _token;

    public GameSceneLoader(ISceneLoader sceneLoader, GameUIController gameUIController, CancellationToken token)
    {
        _sceneLoader = sceneLoader;
        _gameUIController = gameUIController;
        _token = token;
    }

    public void LoadMenu()
    {
        Core().Forget();

        async UniTaskVoid Core()
        {
            await _sceneLoader.LoadAsync<BootstrapScene>(_token); //it should be black fade out, not red
            // await _sceneLoader.LoadAsync<MenuScene>(_token);
            await _sceneLoader.UnloadAsync<GameScene>(_token);
            // await _sceneLoader.LoadAsync<GameScene>(default);
        }
    }

    public void Restart(GameStateMachine gameStateMachine)
    {
        _gameUIController.HideGameOver();
        //black fade out
        //HideGameOver();
        gameStateMachine.Enter<RestartState>();
    }
}