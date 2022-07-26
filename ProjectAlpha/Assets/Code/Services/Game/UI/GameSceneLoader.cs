using System.Threading;
using Cysharp.Threading.Tasks;

namespace Code.Services.Game.UI;

public class GameSceneLoader
{
    private readonly ISceneLoader _sceneLoader;
    private readonly CancellationToken _token;

    public GameSceneLoader(ISceneLoader sceneLoader, CancellationToken token)
    {
        _sceneLoader = sceneLoader;
        _token = token;
    }

    public void LoadMenu()
    {
        Impl().Forget();

        async UniTaskVoid Impl()
        {
            await _sceneLoader.LoadAsync<BootstrapScene>(_token); //it should be black fade out, not red
            // await _sceneLoader.LoadAsync<MenuScene>(_token);
            await _sceneLoader.UnloadAsync<GameScene>(_token);
            // await _sceneLoader.LoadAsync<GameScene>(default);
        }
    }
}
