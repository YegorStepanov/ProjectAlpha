using System;
using Code.Services;
using Code.Services.Data;
using Code.Services.Monetization;
using UnityEngine;
using VContainer.Unity;

namespace Code.Scopes;

public class RootEntryPoint : IInitializable, IDisposable, IStartable
{
    private readonly IProgress _progress;
    private readonly AdsManager _adsManager;
    private readonly ICamera _camera;
    private readonly CameraBackground _cameraBackground;

    public RootEntryPoint(IProgress progress, AdsManager adsManager, ICamera camera, CameraBackground cameraBackground)
    {
        _progress = progress;
        _adsManager = adsManager;
        _camera = camera;
        _cameraBackground = cameraBackground;
    }

    void IInitializable.Initialize()
    {
        _progress.Persistant.IsAdsEnabled.Changed += _adsManager.ShowBannerIfNeeded;
        _progress.Session.RestartNumber.Changed += _adsManager.ShowInterstitialAdIfNeeded;
    }

    void IDisposable.Dispose()
    {
        _progress.Persistant.IsAdsEnabled.Changed -= _adsManager.ShowBannerIfNeeded;
        _progress.Session.RestartNumber.Changed -= _adsManager.ShowInterstitialAdIfNeeded;
    }

    void IStartable.Start()
    {
        _adsManager.ShowBannerIfNeeded();

        BindBackgroundToCamera();
    }

    private void BindBackgroundToCamera()
    {
        var camera = (MonoBehaviour)_camera;
        var background = (MonoBehaviour)_cameraBackground;

        background.transform.SetParent(camera.transform, true);
        background.GetComponent<Canvas>().worldCamera = camera.GetComponent<Camera>();
    }
}
