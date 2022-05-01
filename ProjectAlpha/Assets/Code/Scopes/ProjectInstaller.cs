using Code.Game;
using Code.Services;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Code.Scopes;

public sealed class ProjectInstaller : BaseInstaller<ProjectInitializer>
{
    [Required, AssetsOnly, SerializeField]
    private CameraController _cameraController;

    [Required, SerializeField, AssetSelector(Filter = "t:" + nameof(GameSettings))]
    private GameSettings _gameSettings;

    private void Awake() =>
        DOTween.Init();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
    public static void InitUniTaskLoop()
    {
        //var loop = PlayerLoop.GetCurrentPlayerLoop();
        // // minimum is Update | FixedUpdate | LastPostLateUpdate
        //PlayerLoopHelper.Initialize(ref loop, InjectPlayerLoopTimings.Minimum);
    }

    public override void InstallBindings()
    {
        base.InstallBindings();

        RegisterGameSettings();

        RegisterSceneLoader();

        RegisterCamera();

        RegisterGameTriggers();

        RegisterInputManager();

        Container.Bind<StartSceneInformer>().AsSingle().NonLazy();
    }

    private void RegisterGameSettings() =>
        _gameSettings.BindAllSettings(Container);


    private void RegisterCamera() =>
        Container.Bind<CameraController>()
            .FromComponentInNewPrefab(_cameraController)
            .WithGameObjectName("Camera")
            .AsSingle()
            .NonLazy();

    private void RegisterSceneLoader() =>
        Container.Bind<SceneLoader>().AsSingle();

    private void RegisterGameTriggers() =>
        Container.Bind<GameTriggers>().AsSingle();


    private void RegisterInputManager() =>
        Container.Bind<InputManager>().AsSingle();
}