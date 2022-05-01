using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Code.Services;

public sealed class BackgroundChanger : IDisposable
{
    private readonly Address[] _addresses;
    private readonly Image _backgroundImage;
    private readonly AddressableFactory _factory;

    private Sprite _spriteAsset;

    public BackgroundChanger(AddressableFactory factory, Image backgroundImage)
    {
        _factory = factory;
        _backgroundImage = backgroundImage;

        _addresses = new[]
        {
            BackgroundAddress.Background1,
            BackgroundAddress.Background2,
            BackgroundAddress.Background3,
            BackgroundAddress.Background4,
            BackgroundAddress.Background5,
        };
    }

    public void Dispose() =>
        UnloadSpriteImage();

    public async UniTask ChangeToRandomBackgroundAsync()
    {
        Address newAddress = GetRandomBackground();

        var sprite = await _factory.LoadAssetAsync<Sprite>(newAddress);
        _backgroundImage.sprite = sprite;
        _spriteAsset = sprite;
    }

    private Address GetRandomBackground()
    {
        int index = Random.Range(0, _addresses.Length);
        return _addresses[index];
    }

    private void UnloadSpriteImage() =>
        _factory.ReleaseAsset(_spriteAsset);
}