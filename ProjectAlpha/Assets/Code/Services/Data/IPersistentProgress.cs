using System;

namespace Code.Services;

public interface IPersistentProgress
{
    event Action CherriesChanged;
    event Action AdsEnabledChanged;

    int Cherries { get; }
    bool AdsEnabled { get; }

    void RestoreProgressFromDisk();

    void AddCherry();
    void AddCherries(int count);
    void EnableAds();
    void DisableAds();
}
