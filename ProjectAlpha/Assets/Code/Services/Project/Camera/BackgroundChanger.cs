using System;
using Code.AddressableAssets;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Code.Services;

public sealed class BackgroundChanger : IDisposable
{
    //Types=UnityEngine.Texture2D, UnityEngine.Sprite
    //test Sprite.Create()
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

    public void Dispose() =>
        UnloadSpriteImage();

    public async UniTask ChangeToRandomBackgroundAsync()
    {
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

    private void UnloadSpriteImage() =>
        _loader.Release(_spriteAsset); //
}