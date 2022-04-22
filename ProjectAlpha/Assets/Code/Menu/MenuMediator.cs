using Code.Project;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Code.Menu
{
    public sealed class MenuMediator : MonoBehaviour
    {
        [SerializeField] private SceneReference menuScene;
        [SerializeField] private GameObject shopPanelPrefab;

        [Inject] private SceneLoader sceneLoader;
        [Inject] private DiContainer container;
        
        public async UniTaskVoid ShowMenuAsync() => await sceneLoader.LoadAsync(menuScene.ScenePath, default);
        public async UniTaskVoid HideMenuAsync() => await sceneLoader.UnloadAsync(menuScene.ScenePath, default);
        
        public async UniTaskVoid ShowShopPanelAsync()
        {
            var a = await Resources.LoadAsync<GameObject>("ShopMenu");
            var b = (GameObject)a;
            shopPanelPrefab = container.InstantiatePrefab(b);
            // shopPanelPrefab = Instantiate(b);
        }

        public async UniTaskVoid HideShopPanelAsync()
        {
            Destroy(shopPanelPrefab);
            await UniTask.CompletedTask;
        // await sceneLoader.UnloadAsync(shopPanelScene.ScenePath, default);
        }
    }
}