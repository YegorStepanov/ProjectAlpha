using System.Collections.Generic;
using System.Threading;
using Code.AddressableAssets;
using Code.Extensions;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Services.Infrastructure;

public sealed class BackgroundChanger
{
    private readonly IReadOnlyList<Address<Texture2D>> _backgrounds;
    private readonly RawImage _image;
    private readonly IScopedAddressablesLoader _loader;
    private readonly IRandomizer _randomizer;

    private Texture2D _spriteAsset;

    private int _lastIndex = -1;

    public BackgroundChanger(
        IReadOnlyList<Address<Texture2D>> backgrounds,
        IScopedAddressablesLoader loader,
        IRandomizer randomizer,
        ICamera camera)
    {
        _backgrounds = backgrounds;
        _loader = loader;
        _randomizer = randomizer;
        _image = camera.BackgroundImage;
    }

    public async UniTask ChangeBackgroundAsync()
    {
        if (_spriteAsset != null)
            _loader.Release(_spriteAsset);

        Address<Texture2D> background = GetRandomBackground();

        Texture2D texture = await _loader.LoadAssetAsync(background);
        _image.texture = texture;
        _spriteAsset = texture;
    }

    public UniTask MoveBackgroundAsync(CancellationToken stopToken) => _image
        .DOMoveUVX(1f, 0.05f)
        .SetRelative()
        .SetSpeedBased()
        .WithCancellation(stopToken);

    private Address<Texture2D> GetRandomBackground()
    {
        _lastIndex = GetNextIndex();
        return _backgrounds[_lastIndex];
    }

    private int GetNextIndex() => _lastIndex == -1
        ? _randomizer.Next(_backgrounds.Count)
        : _randomizer.NextExcept(_backgrounds.Count, _lastIndex);
}
