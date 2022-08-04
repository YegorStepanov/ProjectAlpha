using Cysharp.Threading.Tasks;

namespace Code.UI;

public interface ILoadingScreen
{
    UniTask FadeInAsync();
    UniTask FadeOutAsync();
}
