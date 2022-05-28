using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Code.Services;

public sealed class CherryController : MonoBehaviour, ICherryController
{
    [SerializeField] private Transform _transform;
    [SerializeField] private SpriteRenderer _sprite;
    private CancellationToken _token;

    public Borders Borders => _sprite.bounds.AsBorders();

    private void Awake() =>
        _token = this.GetCancellationTokenOnDestroy();

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

    public void TeleportTo(Vector2 position, Relative relative) =>
        _transform.position = Borders.GetRelativePoint(position, relative);
}