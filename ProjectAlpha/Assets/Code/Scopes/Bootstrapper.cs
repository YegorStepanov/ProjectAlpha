using System.Threading;
using Code.Services;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Scopes;

public sealed class Bootstrapper : MonoBehaviour
{
    private const string BootstrapSceneName = "Bootstrap";

    private async UniTaskVoid Awake()
    {
        CancellationToken token = this.GetCancellationTokenOnDestroy();
        
        var sceneLoader = new SceneLoader();
        await sceneLoader.LoadAsync<ProjectScene>(token);

        await SceneManager.UnloadSceneAsync(BootstrapSceneName).WithCancellation(token);
    }
}