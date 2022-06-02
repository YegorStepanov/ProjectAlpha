using System.Threading;
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

    public Borders Borders => _sprite.bounds.AsBorders();

    [Inject, UsedImplicitly]
    private void Construct(GameMediator gameMediator, CherrySpawner cherrySpawner, CancellationToken token)
    {
        _gameMediator = gameMediator;
        _cherrySpawner = cherrySpawner;
        _token = token;
    }

    public UniTask MoveRandomlyAsync(IPlatform leftPlatform, float rightPlatformLeftBorder)
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
        _transform.position = position.Shift(Borders, relative);
    }

    public void PickUp()
    {
        _gameMediator.IncreaseCherryCount();
        _cherrySpawner.Despawn(this);
        //show super effect
    }
}