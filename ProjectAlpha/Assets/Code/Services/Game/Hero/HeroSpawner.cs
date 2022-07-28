using UnityEngine;

namespace Code.Services;

public sealed class HeroSpawner
{
    private readonly Hero _hero;

    public HeroSpawner(Hero hero) =>
        _hero = hero;

    public IHero Create(Vector2 position, Relative relative)
    {
        //mb replace it to array with Length=1 like another spawners?
        _hero.ResetState();
        _hero.SetPosition(position, relative);
        return _hero;
    }
}
