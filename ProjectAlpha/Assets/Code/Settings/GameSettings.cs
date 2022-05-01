using Code.Services;
using UnityEngine;
using Zenject;

namespace Code.Game;

[CreateAssetMenu(menuName = "SO/Game Settings")]
public sealed class GameSettings : ScriptableObject
{
    [SerializeField] private HeroController.Settings hero;
    [SerializeField] private PlatformController.Settings platform;
    [SerializeField] private StickController.Settings stick;
    [SerializeField] private StickSpawner.Settings stickSpawner;
    
    public void BindAllSettings(DiContainer container)
    {
        container.BindInstance(hero);
        container.BindInstance(platform);
        container.BindInstance(stick);
        container.BindInstance(stickSpawner);
    }
}