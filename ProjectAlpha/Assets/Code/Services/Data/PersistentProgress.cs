using System;
using UnityEngine;

namespace Code.Services;

public class PersistentProgress : IPersistentProgress
{
    private const string CherriesKey = "Cherries";
    private const string AdsEnabledKey = "AdsEnabled";

    public event Action CherriesChanged;
    public event Action AdsEnabledChanged;

    public int Cherries { get; private set; }
    public bool AdsEnabled { get; private set; }

    public void RestoreProgressFromDisk()
    {
        Cherries = LoadCherries();
        AdsEnabled = LoadAdsEnabled();
    }

    public void AddCherry()
    {
        AddCherries(1);
    }

    public void AddCherries(int count)
    {
        Cherries += count;
        SaveCherries(Cherries);
        CherriesChanged?.Invoke();
    }

    public void EnableAds()
    {
        AdsEnabled = true;
        SaveAdsEnabled(AdsEnabled);
        AdsEnabledChanged?.Invoke();
    }

    public void DisableAds()
    {
        AdsEnabled = false;
        SaveAdsEnabled(AdsEnabled);
        AdsEnabledChanged?.Invoke();
    }

    private static int LoadCherries() =>
        PlayerPrefs.GetInt(CherriesKey, 0);

    private static void SaveCherries(int value) =>
        PlayerPrefs.SetInt(CherriesKey, value);

    private static bool LoadAdsEnabled() =>
        PlayerPrefs.GetInt(AdsEnabledKey, 0) == 0;

    private static void SaveAdsEnabled(bool value) =>
        PlayerPrefs.SetInt(AdsEnabledKey, value ? 0 : 1);
}
