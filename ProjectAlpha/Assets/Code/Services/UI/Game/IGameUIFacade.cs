namespace Code.Services.UI;

public interface IGameUIFacade
{
    void RequestStoreReview();
    void HideGameOver();
    void LoadMenu();
    void Restart();
}
