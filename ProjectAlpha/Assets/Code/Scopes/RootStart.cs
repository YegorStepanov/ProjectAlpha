using System.Threading;
using Code.Services;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine.AddressableAssets;
using VContainer.Unity;

namespace Code.Scopes;

public class RootStart : IAsyncStartable
{
    private readonly GlobalAddressableFactory _factory;

    public RootStart(GlobalAddressableFactory factory) =>
        _factory = factory;

    public async UniTask StartAsync(CancellationToken cancellation)
    {
        DOTween.Init();
        Addressables.InitializeAsync(true);

        await _factory.PreloadAsync(MenuAddress.MainMenu);
    }
}