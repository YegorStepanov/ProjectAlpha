using System.Threading;
using Cysharp.Threading.Tasks;

namespace Code.Services.Game.UI;

public class GameSceneLoader
{
    private readonly ISceneLoader _sceneLoader;
    private readonly CancellationToken _token;

    public GameSceneLoader(ISceneLoader sceneLoader, ScopeCancellationToken token)
    {
        _sceneLoader = sceneLoader;
        _token = token.Token;
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
}
