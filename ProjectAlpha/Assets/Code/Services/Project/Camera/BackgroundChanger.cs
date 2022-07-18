using System.Threading;
using Code.AddressableAssets;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Services;

public sealed class BackgroundChanger
{
    private readonly Address<Texture2D>[] _addresses;
    private readonly RawImage _image;
    private readonly IScopedAddressablesLoader _loader;
    private readonly IRandomizer _randomizer;

    private Texture2D _spriteAsset;

    private int lastIndex = -1;

    public BackgroundChanger(IScopedAddressablesLoader loader, IRandomizer randomizer, RawImage image)
    {
        _loader = loader;
        _randomizer = randomizer;
        _image = image;

        _addresses = new[]
        {
            BackgroundAddress.Background1,
            BackgroundAddress.Background2,
            BackgroundAddress.Background3,
            BackgroundAddress.Background4,
            BackgroundAddress.Background5,
        };
    }

    public async UniTask ChangeToRandomBackgroundAsync()
    {
        if (_spriteAsset != null)
            _loader.Release(_spriteAsset);

        Address<Texture2D> background = GetRandomBackground();

        Texture2D texture = await _loader.LoadAssetAsync(background);
        _image.texture = texture;
        _spriteAsset = texture;
    }

    public UniTask MoveBackgroundAsync(CancellationToken token) => _image
        .DOMoveUVX(1f, 0.05f)
        .SetRelative()
        .SetSpeedBased()
        .WithCancellation(token);

    private Address<Texture2D> GetRandomBackground()
    {
        lastIndex = GetNextIndex();
        return _addresses[lastIndex];
    }

    private int GetNextIndex() => lastIndex == -1
        ? _randomizer.Next(_addresses.Length)
        : _randomizer.NextExcept(_addresses.Length, lastIndex);
}
