using System;
using Code.Services.Data;
using Code.Services.Development;
using Code.Services.Infrastructure;
using Code.Services.Monetization;
using VContainer.Unity;

namespace Code.Scopes;

public class RootEntryPoint : IInitializable, IDisposable, IStartable
{
    private readonly IProgress _progress;
    private readonly AdsManager _adsManager;

    public RootEntryPoint(IProgress progress, AdsManager adsManager, DevelopmentPanel developmentPanel, ICamera camera1)
    {
        _progress = progress;
        _adsManager = adsManager;
        _ = developmentPanel;
    }

    void IInitializable.Initialize()
    {
        _progress.Persistant.RestoreProgressFromDisk();

        _progress.Persistant.AdsEnabledChanged += _adsManager.ShowBannerIfNeeded;
        _progress.Session.RestartNumberChanged += _adsManager.ShowInterstitialAdIfNeeded;
    }

    void IDisposable.Dispose()
    {
        _progress.Persistant.AdsEnabledChanged -= _adsManager.ShowBannerIfNeeded;
        _progress.Session.RestartNumberChanged -= _adsManager.ShowInterstitialAdIfNeeded;
    }

    void IStartable.Start()
    {
        _adsManager.ShowBannerIfNeeded();
    }
}
