using System.Threading;
using Code.AddressableAssets;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine.AddressableAssets;
using VContainer.Unity;

namespace Code.Scopes;

public class RootStart : IAsyncStartable
{
    private readonly IAddressablesCache _cache;

    public RootStart(IAddressablesCache cache) =>
        _cache = cache;

    public async UniTask StartAsync(CancellationToken cancellation)
    {
        DOTween.Init();
        Addressables.InitializeAsync(true);

        await _cache.CacheAssetAsync(MenuAddress.MainMenu);
    }
}