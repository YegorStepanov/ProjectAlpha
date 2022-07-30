using Code.Services.Infrastructure;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code;

public sealed class Bootstrapper : MonoBehaviour
{
    [UsedImplicitly]
    private async UniTaskVoid Awake()
    {
        await SceneLoader.Instance.LoadAsync<BootstrapScene>(this.GetCancellationTokenOnDestroy());

        await SceneManager.UnloadSceneAsync(gameObject.scene);
    }
}
