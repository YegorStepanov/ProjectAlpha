using System.Threading;
using Code.Services;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine.AddressableAssets;
using VContainer.Unity;

namespace Code.Scopes;

public class RootStart : IAsyncStartable
{
    private readonly GlobalAddressableLoader _loader;

    public RootStart(GlobalAddressableLoader loader) =>
        _loader = loader;

    public async UniTask StartAsync(CancellationToken cancellation)
    {
        DOTween.Init();
        Addressables.InitializeAsync(true);

        await _loader.PreloadAsync(MenuAddress.MainMenu);
    }
}