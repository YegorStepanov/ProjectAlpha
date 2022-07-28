namespace Code.Services.Game.UI;

public interface IGameUIMediator
{
    void RequestStoreReview();
    void HideGameOver();
    void LoadMenu();
    void Restart();
}
