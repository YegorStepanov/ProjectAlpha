using System.Collections.Generic;
using System.Threading;
using Code.AddressableAssets;
using Code.Extensions;
using Code.Services.Infrastructure;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Code.Services;

public sealed class CameraBackground : MonoBehaviour
{
    [SerializeField] private RawImage _rawImage;

    private IReadOnlyList<Address<Texture2D>> _backgroundAddresses;
    private IScopedAddressablesLoader _loader;
    private IRandomizer _randomizer;

    private Texture2D _currentTexture;
    private int _lastIndex = -1;

    [Inject, UsedImplicitly]
    public void Construct(
        IReadOnlyList<Address<Texture2D>> addresses,
        IScopedAddressablesLoader loader,
        IRandomizer randomizer)
    {
        _backgroundAddresses = addresses;
        _loader = loader;
        _randomizer = randomizer;
    }

    public async UniTask ChangeBackgroundAsync()
    {
        if (_rawImage != null)
            _loader.Release(_currentTexture);

        Address<Texture2D> address = GetRandomBackground();
        _currentTexture = await _loader.LoadAssetAsync(address);
        _rawImage.texture = _currentTexture;
    }

    public UniTask MoveBackgroundAsync(CancellationToken stopToken) => _rawImage
        .DOMoveUVX(1f, 0.05f)
        .SetRelative()
        .SetSpeedBased()
        .WithCancellation(stopToken);

    private Address<Texture2D> GetRandomBackground()
    {
        _lastIndex = _randomizer.NextExcept(_backgroundAddresses.Count, _lastIndex);
        return _backgroundAddresses[_lastIndex];
    }
}
