using System;
using Code.Services;
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

    public void Initialize()
    {
        _progress.Persistant.RestoreProgressFromDisk();

        _progress.Persistant.AdsEnabledChanged += _adsManager.ShowBannerIfNeeded;
        _progress.Session.RestartNumberChanged += _adsManager.ShowInterstitialAdIfNeeded;
    }

    public void Dispose()
    {
        _progress.Persistant.AdsEnabledChanged -= _adsManager.ShowBannerIfNeeded;
        _progress.Session.RestartNumberChanged -= _adsManager.ShowInterstitialAdIfNeeded;
    }

    public void Start()
    {
        _adsManager.ShowBannerIfNeeded();
    }
}
