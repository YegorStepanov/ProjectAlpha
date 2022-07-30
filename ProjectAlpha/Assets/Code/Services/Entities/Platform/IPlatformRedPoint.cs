using Cysharp.Threading.Tasks;

namespace Code.Services.Entities.Platform;

public interface IPlatformRedPoint : IEntity
{
    void Toggle(bool enable);
    UniTask FadeOutAsync();
}
