﻿using System;
using Code.AddressableAssets;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Code.Services;

[RequireComponent(typeof(Camera))]
public sealed class CameraController : MonoBehaviour, IDisposable
{
    [Required, SerializeField] private Camera _baseCamera;
    [Required, SerializeField] private Image _backgroundImage;

    private BackgroundChanger _backgroundChanger;

    public Borders Borders => UpdateBorders();

    public Vector3 CameraPosition => _baseCamera.transform.position;

    public void Dispose() =>
        _backgroundChanger?.Dispose();

    [Inject, UsedImplicitly]
    public void Construct(IScopedAddressablesLoader loader)
    {
        _backgroundChanger = new BackgroundChanger(loader, _backgroundImage);
    }

    public UniTask ChangeBackgroundAsync() =>
        _backgroundChanger.ChangeToRandomBackgroundAsync();

    //
    // void IInitializable.Initialize()
    // {
    //     // UpdateBorders(screenSizeChecker.ScreenSize);
    // }
    //
    // void IDisposable.Dispose()
    // {
    //     // screenSizeChecker.OnScreenResized -= UpdateBorders;
    // }

    public async UniTask MoveAsync(Vector2 destination) =>
        await MoveAsync(destination.WithZ(CameraPosition.z));

    private async UniTask MoveAsync(Vector3 destination) =>
        await _baseCamera.transform.DOMove(destination, 7f).SetSpeedBased();

    private Borders UpdateBorders()
    {
        Vector2 topRightCorner = _baseCamera.ViewportToWorldPoint(Vector2.one);
        Vector2 bottomLeftCorner = _baseCamera.ViewportToWorldPoint(Vector2.zero);

        return new Borders(
            Top: topRightCorner.y,
            Right: topRightCorner.x,
            Bottom: bottomLeftCorner.y,
            Left: bottomLeftCorner.x
        );
    }

    public Vector2 ViewportToWorldPosition(Vector2 viewportPosition) =>
        _baseCamera.ViewportToWorldPoint(viewportPosition);
}