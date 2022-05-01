using Code.Services;
using UnityEngine;
using Zenject;

namespace Code.Game;

[CreateAssetMenu(menuName = "SO/Game Settings")]
public sealed class GameSettings : ScriptableObject
{
    [SerializeField] private HeroController.Settings _hero;
    [SerializeField] private PlatformController.Settings _platform;
    [SerializeField] private StickController.Settings _stick;
    [SerializeField] private StickSpawner.Settings _stickSpawner;
    
    public void BindAllSettings(DiContainer container)
    {
        container.BindInstance(_hero);
        container.BindInstance(_platform);
        container.BindInstance(_stick);
        container.BindInstance(_stickSpawner);
    }
}