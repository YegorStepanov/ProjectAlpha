﻿using Code.AddressableAssets;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Services;

public sealed class BackgroundChanger
{
    private readonly Address<Sprite>[] _addresses;
    private readonly Image _backgroundImage;
    private readonly IScopedAddressablesLoader _loader;

    private Sprite _spriteAsset;

    public BackgroundChanger(IScopedAddressablesLoader loader, Image backgroundImage)
    {
        _loader = loader;
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

    public async UniTask ChangeToRandomBackgroundAsync()
    {
        if (_spriteAsset != null)
            _loader.Release(_spriteAsset);

        Address<Sprite> newAddress = GetRandomBackground();

        Sprite sprite = await _loader.LoadAssetAsync(newAddress);
        _backgroundImage.sprite = sprite;
        _spriteAsset = sprite;
    }

    private Address<Sprite> GetRandomBackground()
    {
        int index = Random.Range(0, _addresses.Length);
        return _addresses[index];
    }
}