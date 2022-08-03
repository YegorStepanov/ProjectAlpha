using System.Threading;
using Cysharp.Threading.Tasks;

namespace Code.Services.Infrastructure;

public class GameSceneNavigator : IGameSceneNavigator
{
    private readonly ISceneLoader _sceneLoader;
    private readonly CancellationToken _token;

    public GameSceneNavigator(ISceneLoader sceneLoader, CancellationToken token)
    {
        _sceneLoader = sceneLoader;
        _token = token;
    }

    public void NavigateToMenu() =>
        NavigateToMenuImpl().Forget();

    private async UniTaskVoid NavigateToMenuImpl()
    {
        //todo: replace it to menuScene
        await _sceneLoader.LoadAsync<BootstrapScene>(_token); //it should be black fade out, not red
        // await _sceneLoader.LoadAsync<MenuScene>(_token);
        await _sceneLoader.UnloadAsync<GameScene>(_token);
        // await _sceneLoader.LoadAsync<GameScene>(default);
    }
}
