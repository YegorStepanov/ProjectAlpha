using DG.Tweening;
using UnityEngine.AddressableAssets;
using VContainer.Unity;

namespace Code.Scopes;

public class RootStart : IStartable
{
    public void Start()
    {
        DOTween.Init();
        Addressables.InitializeAsync(true);
    }
}