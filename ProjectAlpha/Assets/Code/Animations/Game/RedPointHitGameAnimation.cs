using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Code.Animations.Game;

public sealed class RedPointHitGameAnimation : MonoBehaviour
{
    [SerializeField] private Vector2 _offset = new(0, 0.1f);

    public async UniTask PlayAsync(Vector2 position, CancellationToken token)
    {
        gameObject.SetActive(true);

        transform.position = position + _offset;

        await transform.DOMoveY(0.5f, 1f)
            .SetRelative()
            .WithCancellation(token);

        gameObject.SetActive(false);
    }
}