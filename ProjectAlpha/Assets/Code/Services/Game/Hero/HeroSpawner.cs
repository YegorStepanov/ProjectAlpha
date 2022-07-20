using UnityEngine;

namespace Code.Services;

public sealed class HeroSpawner
{
    private readonly Hero _hero;

    public HeroSpawner(Hero hero) =>
        _hero = hero;

    public IHero Create(Vector2 position, Relative relative)
    {
        _hero.ResetState();
        _hero.SetPosition(position, relative);
        return _hero;
    }
}
