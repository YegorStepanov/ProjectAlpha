﻿using System.Threading;
using Code.Services.Game.UI;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;
using VContainer;

namespace Code.Services;

public sealed class Cherry : MonoBehaviour, ICherry
{
    [SerializeField] private Transform _transform;
    [SerializeField] private SpriteRenderer _sprite;

    private GameMediator _gameMediator;
    private CancellationToken _token;
    private CherrySpawner _cherrySpawner;
    private Settings _settings;

    public Borders Borders => _sprite.bounds.AsBorders();

    [Inject, UsedImplicitly]
    private void Construct(GameMediator gameMediator, CherrySpawner cherrySpawner, Settings settings)
    {
        _gameMediator = gameMediator;
        _cherrySpawner = cherrySpawner;
        _settings = settings;
    }
    
    private void Awake() =>
        _token = this.GetCancellationTokenOnDestroy();

    public void SetPosition(Vector2 position, Relative relative) =>
        _transform.position = position.Shift(Borders, relative);

    public UniTask MoveRandomlyAsync(IPlatform leftPlatform, float rightPlatformLeftBorder)
    {
        float min = leftPlatform.Borders.Right + Borders.HalfWidth;
        float max = rightPlatformLeftBorder - Borders.HalfWidth;

        float endDestination = Random.Range(min, max);

        return _transform.DOMoveX(endDestination, _settings.MovementSpeed)
            .SetEase(Ease.OutQuad)
            .SetSpeedBased()
            .WithCancellation(_token);
    }

    public void PickUp()
    {
        _gameMediator.IncreaseCherryCount();
        _cherrySpawner.Despawn(this);
        //show super effect
    }

    [System.Serializable]
    public class Settings
    {
        public float MovementSpeed = 10f;
    }
}