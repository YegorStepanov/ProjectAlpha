using System;
using Code.Game;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Code.Services;

[RequireComponent(typeof(Camera))]
public sealed class CameraController : MonoBehaviour, IDisposable
{
    //public record struct LLL;

    [Required, SerializeField] private Camera baseCamera;
    [Required, SerializeField] private Image backgroundImage;

    private BackgroundChanger backgroundChanger;


    public Borders Borders => UpdateBorders();

    public Vector3 CameraPosition => baseCamera.transform.position;

    public void Dispose() =>
        backgroundChanger?.Dispose();

    [Inject]
    public void Construct(AddressableFactory factory) =>
        backgroundChanger = new BackgroundChanger(factory, backgroundImage);

    public UniTask ChangeBackgroundAsync() =>
        backgroundChanger.ChangeToRandomBackgroundAsync();

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

    public async UniTask ShiftAsync(Vector2 offset) =>
        await MoveAsync(CameraPosition.ShiftXY(-offset));

    public async UniTask MoveAsync(float destinationX, Relative relative = Relative.Center)
    {
        float finalX = Borders.TransformPointX(destinationX, relative);
        await MoveAsync(CameraPosition.WithX(finalX));
    }

    public async UniTask MoveAsync(Vector2 destination, Relative relative = Relative.Center)
    {
        Vector2 finalDestination = GetRelativePosition(destination, relative);
        await MoveAsync(CameraPosition.WithXY(finalDestination));
    }

    public Vector2 GetRelativePosition(Vector2 position, Relative relative) =>
        Borders.TransformPoint(position, relative);

    private async UniTask MoveAsync(Vector3 destination) =>
        await baseCamera.transform.DOMove(destination, 7f).SetSpeedBased();

    private Borders UpdateBorders()
    {
        Vector2 topRightCorner = baseCamera.ViewportToWorldPoint(Vector2.one);
        Vector2 bottomLeftCorner = baseCamera.ViewportToWorldPoint(Vector2.zero);

        return new Borders(
            Top: topRightCorner.y,
            Right: topRightCorner.x,
            Bottom: bottomLeftCorner.y,
            Left: bottomLeftCorner.x
        );
    }

    public Vector2 ViewportToWorldPosition(Vector2 viewportPosition) =>
        baseCamera.ViewportToWorldPoint(viewportPosition);

    // public float WorldHeight() =>

    //     camera.orthographicSize * 2;

    //

    // public float WorldWidth() =>

    //     camera.orthographicSize * camera.aspect * 2;


    private float OffsetToLeftCameraBorder() =>
        -ViewportToWorldPosition(Vector2.zero).x;

    private float MenuPlatformOffsetToLeftBorder() => 0f;

    // -gameSettings.MenuPlatformWidth / 2f;


    private float MenuToGamePlatformOffsetY() => 0f;

    // gameSettings.GamePlatformPositionY - gameSettings.MenuPlatformPositionY;
}