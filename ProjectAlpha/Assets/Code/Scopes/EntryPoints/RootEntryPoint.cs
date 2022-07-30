using System;
using Code.Services.Data;
using Code.Services.Development;
using Code.Services.Infrastructure;
using Code.Services.Monetization;
using UnityEngine;
using VContainer.Unity;

namespace Code.Scopes.EntryPoints;

public class RootEntryPoint : IInitializable, IDisposable, IStartable
{
    private readonly IProgress _progress;
    private readonly AdsManager _adsManager;

    public RootEntryPoint(IProgress progress, AdsManager adsManager, DevelopmentPanel developmentPanel, ICamera camera1)
    {
        Debug.Log("RootEntryPoint");
        _progress = progress;
        _adsManager = adsManager;
        _ = developmentPanel;
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
