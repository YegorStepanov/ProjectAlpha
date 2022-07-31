using Cysharp.Threading.Tasks;

namespace Code.Services.Entities;

public interface IPlatformRedPoint : IEntity
{
    void Toggle(bool enable);
    UniTask FadeOutAsync();
}
