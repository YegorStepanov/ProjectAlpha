using System.Collections.Generic;
using Code.AddressableAssets;
using Code.Services.Data;
using Code.Services.Entities;
using UnityEngine;

namespace Code.Services;

public class HeroSelector
{
    private readonly IReadOnlyList<Address<Hero>> _addresses;
    private readonly IProgress _progress;

    public HeroSelector(IReadOnlyList<Address<Hero>> heroes, IProgress progress)
    {
        _progress = progress;
        _addresses = heroes;
    }

    public Address<Hero> GetSelectedHero()
    {
        int index = _progress.Persistant.SelectedHeroIndex;
        index--;

        if (index < 0 || index >= _addresses.Count)
            Debug.LogError($"Index is out of array. Index={index} length={_addresses.Count}");

        return _addresses[index];
    }
}
