using Cysharp.Threading.Tasks;

namespace Code.Services.Entities;

public interface IPlatformRedPoint : IEntity
{
    void ToggleVisibility(bool enable);
    UniTask FadeOutAsync();
}
