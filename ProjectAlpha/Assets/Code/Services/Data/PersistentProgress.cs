using System;

namespace Code.Services.Data
{
    public class PersistentProgress : IPersistentProgress
    {
        private readonly PersistentValueWriter<int> _cherries = PersistentValueFactory.CreateInt(0, "Cherries");
        private readonly PersistentValueWriter<bool> _isAdsEnabled = PersistentValueFactory.CreateBool(true, "AdsEnabled");
        private readonly PersistentValueWriter<int> _selectedHeroIndex = PersistentValueFactory.CreateInt(1, "SelectedHeroIndex");
        private readonly PersistentValueWriter<bool> _isHero1Locked = PersistentValueFactory.CreateBool(false, "Hero1Locked");
        private readonly PersistentValueWriter<bool> _isHero2Locked = PersistentValueFactory.CreateBool(true, "Hero2Locked");
        private readonly PersistentValueWriter<bool> _isHero3Locked = PersistentValueFactory.CreateBool(true, "Hero3Locked");
        private readonly PersistentValueWriter<bool> _isHero4Locked = PersistentValueFactory.CreateBool(true, "Hero4Locked");

        public ObservedValue<int> Cherries => _cherries;
        public ObservedValue<bool> IsAdsEnabled => _isAdsEnabled;
        public ObservedValue<int> SelectedHeroIndex => _selectedHeroIndex;

        public void AddCherries(int count) => _cherries.Value += count;
        public void EnableAds() => _isAdsEnabled.Value = true;
        public void DisableAds() => _isAdsEnabled.Value = false;

        public void UnlockHero(int heroIndex) => GetIsHeroLocked(heroIndex).Value = false;
        public bool IsHeroLocked(int heroIndex) => GetIsHeroLocked(heroIndex).Value;

        public void SetSelectedHero(int heroIndex)
        {
            if (heroIndex is 1 or 2 or 3 or 4)
                _selectedHeroIndex.Value = heroIndex;
            else
                throw new ArgumentOutOfRangeException($"Index is out of the array. Index={heroIndex} length=4");
        }

        private PersistentValueWriter<bool> GetIsHeroLocked(int index) => index switch
        {
            1 => _isHero1Locked,
            2 => _isHero2Locked,
            3 => _isHero3Locked,
            4 => _isHero4Locked,
            _ => throw new ArgumentOutOfRangeException(nameof(index), index, null)
            };
    }
}
