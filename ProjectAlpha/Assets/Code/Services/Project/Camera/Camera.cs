using System.Threading;
using Code.AddressableAssets;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Code.Services;

[RequireComponent(typeof(UnityEngine.Camera))]
public sealed class Camera : MonoBehaviour, IEntity
{
    [Required, SerializeField] private UnityEngine.Camera _baseCamera;
    [Required, SerializeField] private RawImage _backgroundImage;

    private BackgroundChanger _backgroundChanger;
    private Vector3 _initialPosition;
    private Settings _settings;

    public Borders Borders => UpdateBorders();

    private Vector3 CameraPosition => _baseCamera.transform.position;

    [Inject, UsedImplicitly]
    public void Construct(IScopedAddressablesLoader loader, IRandomizer randomizer, Settings settings)
    {
        _settings = settings;
        //it should be null, isn't?
        _backgroundChanger = new BackgroundChanger(loader, randomizer, _backgroundImage);
    }

    private void Awake() =>
        _initialPosition = transform.position;

    public void RestoreInitialPosition() =>
        transform.position = _initialPosition;

    public UniTask ChangeBackgroundAsync() =>
        _backgroundChanger.ChangeToRandomBackgroundAsync();

    public UniTask MoveBackgroundAsync(CancellationToken cancellationToken) =>
        _backgroundChanger.MoveBackgroundAsync(cancellationToken);

    //todo: PunchAsync?
    public UniTask Punch(CancellationToken token) => transform
        .DOPunchPosition(
            _settings.PunchingStrength,
            _settings.PunchingDuration,
            _settings.PunchingVibrato,
            _settings.PunchingElasticity)
        .WithCancellation(token);

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
