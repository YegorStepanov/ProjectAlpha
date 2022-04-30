using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Code.Services;

public sealed class BackgroundChanger : IDisposable
{
    private readonly Address[] addresses;
    private readonly Image backgroundImage;
    private readonly AddressableFactory factory;

    private Sprite spriteAsset;

    public BackgroundChanger(AddressableFactory factory, Image backgroundImage)
    {
        this.factory = factory;
        this.backgroundImage = backgroundImage;

        addresses = new[]
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

        var sprite = await factory.LoadAssetAsync<Sprite>(newAddress);
        backgroundImage.sprite = sprite;
        spriteAsset = sprite;
    }

    private Address GetRandomBackground()
    {
        int index = Random.Range(0, addresses.Length);
        return addresses[index];
    }

    private void UnloadSpriteImage() =>
        factory.ReleaseAsset(spriteAsset);
}