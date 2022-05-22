using Code.Services;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code;

public sealed class Bootstrapper : MonoBehaviour
{
    private async UniTaskVoid Awake()
    {
        await SceneLoader.Instance.LoadAsync<BootstrapScene>(this.GetCancellationTokenOnDestroy());
        
        await SceneManager.UnloadSceneAsync(gameObject.scene);
    }
}