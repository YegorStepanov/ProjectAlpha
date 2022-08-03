namespace Code.Services.Data;

public interface IPersistentProgress
{
    ObservedValue<int> Cherries { get; }
    ObservedValue<bool> IsAdsEnabled { get; }
    ObservedValue<int> SelectedHeroIndex { get; }

    void AddCherries(int count);
    void EnableAds();
    void DisableAds();

    //refactor it
    void UnlockHero(int heroIndex);
    bool IsHeroLocked(int heroIndex);
    void SetSelectedHero(int heroIndex);
}
