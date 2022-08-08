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
public sealed class CameraController : MonoBehaviour, ICamera
{
    [Required, SerializeField] private Camera _baseCamera;
    [Required, SerializeField] private RawImage _backgroundImage;

    private BackgroundChanger _backgroundChanger;
    private Settings _settings;

    public RawImage BackgroundImage => _backgroundImage;
    public Borders Borders => UpdateBorders();

    [Inject, UsedImplicitly]
    private void Construct(Settings settings) =>
        _settings = settings;

    public void SetPosition(Vector2 position) =>
        transform.position = transform.position.WithXY(position);

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

    public UniTask MoveAsync(Vector2 destination) => _baseCamera.transform
        .DOMove(_baseCamera.transform.position.WithXY(destination), 7f)
        .SetSpeedBased()
        .ToUniTask();

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

    [System.Serializable]
    public class Settings
    {
        public Vector3 PunchingStrength = Vector3.up;
        public float PunchingDuration = 0.3f;
        public int PunchingVibrato = 10;
        public float PunchingElasticity = 1f;
    }
}
