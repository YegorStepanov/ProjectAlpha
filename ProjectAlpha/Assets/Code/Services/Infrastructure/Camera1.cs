using Code.AddressableAssets;
using Code.Common;
using Code.Extensions;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Code.Services.Infrastructure;

[RequireComponent(typeof(Camera))]
public sealed class Camera1 : MonoBehaviour, ICamera
{
    [Required, SerializeField] private Camera _baseCamera;
    [Required, SerializeField] private RawImage _backgroundImage;

    private BackgroundChanger _backgroundChanger;
    private Vector3 _initialPosition;
    private Settings _settings;

    public Borders Borders => UpdateBorders();

    private Vector3 CameraPosition => _baseCamera.transform.position;

    [Inject, UsedImplicitly]
    private void Construct(IScopedAddressablesLoader loader, IRandomizer randomizer, Settings settings)
    {
        _settings = settings;
        _backgroundChanger = new BackgroundChanger(loader, randomizer, _backgroundImage);
    }

    private void Awake() => //to Progress!
        _initialPosition = transform.position;

    public void RestoreInitialPosX() => //to Progress!
        transform.position = transform.position.WithX(_initialPosition.x);

    public UniTask ChangeBackgroundAsync() =>
        _backgroundChanger.ChangeToRandomBackgroundAsync();

    public UniTask MoveBackgroundAsync() =>
        _backgroundChanger.MoveBackgroundAsync();

    public async UniTask PunchAsync() => await transform.DOPunchPosition(
        _settings.PunchingStrength,
        _settings.PunchingDuration,
        _settings.PunchingVibrato,
        _settings.PunchingElasticity);

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
        Vector2 rightTopCorner = _baseCamera.ViewportToWorldPoint(Vector2.one);
        Vector2 leftBotCorner = _baseCamera.ViewportToWorldPoint(Vector2.zero);

        return new Borders(
            Top: rightTopCorner.y,
            Right: rightTopCorner.x,
            Bot: leftBotCorner.y,
            Left: leftBotCorner.x
        );
    }

    public Vector2 ViewportToWorldPosition(Vector2 viewportPosition) =>
        _baseCamera.ViewportToWorldPoint(viewportPosition);

    public float ViewportToWorldPositionX(float viewportPosX) =>
        _baseCamera.ViewportToWorldPoint(new Vector2(viewportPosX, 0)).x;

    public float ViewportToWorldPositionY(float viewportPosY) =>
        _baseCamera.ViewportToWorldPoint(new Vector2(0, viewportPosY)).y;

    [System.Serializable]
    public class Settings
    {
        public Vector3 PunchingStrength = Vector3.up;
        public float PunchingDuration = 0.3f;
        public int PunchingVibrato = 10;
        public float PunchingElasticity = 1f;
    }
}
