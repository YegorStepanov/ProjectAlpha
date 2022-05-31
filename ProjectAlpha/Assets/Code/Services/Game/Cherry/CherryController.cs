using System.Threading;
using Code.Services.Game.UI;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;
using VContainer;

namespace Code.Services;

public sealed class CherryController : MonoBehaviour, ICherryController
{
    [SerializeField] private Transform _transform;
    [SerializeField] private SpriteRenderer _sprite;

    private StickSpawner _stickSpawner;
    private CancellationToken _token;
    private GameUIMediator _gameUI;

    public Borders Borders => _sprite.bounds.AsBorders();

    [Inject, UsedImplicitly]
    private void Construct(StickSpawner stickSpawner, CancellationToken token, GameUIMediator gameUI)
    {
        _stickSpawner = stickSpawner;
        _token = token;
        _gameUI = gameUI;
    }

    public UniTask MoveRandomlyAsync(IPlatformController leftPlatform, float rightPlatformLeftBorder)
    {
        float min = leftPlatform.Borders.Right + Borders.Width / 2f;
        float max = rightPlatformLeftBorder - Borders.Width / 2f;

        float endDestination = Random.Range(min, max);

        return _transform.DOMoveX(endDestination, 10f)
            .SetEase(Ease.OutQuad)
            .SetSpeedBased()
            .WithCancellation(_token);
    }

    public void TeleportTo(Vector2 position, Relative relative)
    {
        position.y -= _stickSpawner.StickWidth;
        _transform.position = position.Shift(Borders, relative);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Hero")) return;
        _gameUI.IncreaseCherryCount();
    }
}