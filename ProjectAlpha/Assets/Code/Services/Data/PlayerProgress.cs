using System;
using UnityEngine;

namespace Code.Services;

public class PlayerProgress
{
    private const string CherryCountKey = "CherryCount";
    private const string IsNoAdsKey = "IsNoAds";

    public event Action<int> CherryCountChanged;
    public event Action<bool> IsNoAdsChanged;

    public int CherryCount { get; private set; }
    public bool IsNoAds { get; private set; }

    public PlayerProgress() =>
        Initialize();

    private void Initialize()
    {
        CherryCount = LoadCherryCount();
        IsNoAds = LoadIsNoAds();
    }

    public void IncreaseCherryCount()
    {
        AddCherryCount(1);
    }

    public void AddCherryCount(int count)
    {
        CherryCount += count;
        SaveCherryCount(CherryCount);
        CherryCountChanged?.Invoke(CherryCount);
    }

    public void SetNoAds()
    {
        IsNoAds = true;
        SaveIsNoAds(IsNoAds);
        IsNoAdsChanged?.Invoke(IsNoAds);
    }

    private static int LoadCherryCount() =>
        PlayerPrefs.GetInt(CherryCountKey, 0);

    private static void SaveCherryCount(int value) =>
        PlayerPrefs.SetInt(CherryCountKey, value);

    private static bool LoadIsNoAds() =>
        PlayerPrefs.GetInt(IsNoAdsKey, 0) != 0;

    private static void SaveIsNoAds(bool value) =>
        PlayerPrefs.SetInt(IsNoAdsKey, value ? 1 : 0);
}
