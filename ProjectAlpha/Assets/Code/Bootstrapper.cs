using Code.Services;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code;

public sealed class Bootstrapper : MonoBehaviour
{
    public Tween tween;
    
    [UsedImplicitly]
    private async UniTaskVoid Awake()
    {
        await SceneLoader.Instance.LoadAsync<BootstrapScene>(this.GetCancellationTokenOnDestroy());

        await SceneManager.UnloadSceneAsync(gameObject.scene);
    }
}