using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Code.Services;

public sealed class StickSpawner
{
    private readonly Transform _scopeTransform;
    private readonly IObjectResolver _resolver;
    private readonly StickController _stickPrefab;
    private readonly Settings _settings;

    public StickSpawner(LifetimeScope scope, IObjectResolver resolver, StickController stickPrefab, Settings settings)
    {
        _scopeTransform = scope.transform;
        _resolver = resolver;
        _stickPrefab = stickPrefab;
        _settings = settings;
    }

    public float StickWidth => _settings.StickWidth;

    public IStickController Spawn(float positionX)
    {
        StickController stick = _resolver.Instantiate(_stickPrefab, _scopeTransform, true); //correct form!
        stick.transform.SetParent(null);
        
        stick.Position = stick.Position.WithX(positionX);
        stick.Width = _settings.StickWidth;
        return stick;
    }

    [Serializable]
    public class Settings
    {
        public float StickWidth = 0.04f;
    }
}