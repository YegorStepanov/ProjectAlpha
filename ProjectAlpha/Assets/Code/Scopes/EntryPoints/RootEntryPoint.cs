using System;
using Code.Services.Data;
using Code.Services.Monetization;
using VContainer.Unity;

namespace Code.Scopes;

public class RootEntryPoint : IInitializable, IDisposable, IStartable
{
    private readonly IProgress _progress;
    private readonly AdsManager _adsManager;

    public RootEntryPoint(IProgress progress, AdsManager adsManager)
    {
        _progress = progress;
        _adsManager = adsManager;
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
    }
}
