using Cysharp.Threading.Tasks;

namespace Code.Services;

public interface IRedPoint : IEntity
{
    bool Inside(float pointX);
    void Toggle(bool enable);
    UniTask FadeOutAsync();
}