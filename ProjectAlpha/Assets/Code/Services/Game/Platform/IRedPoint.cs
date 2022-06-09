using Cysharp.Threading.Tasks;

namespace Code.Services;

public interface IRedPoint : IEntity
{
    void Toggle(bool enable);
    UniTask FadeOutAsync();
}