using Code.Scopes;
using Code.Services.Data;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Code.Services.Development;

public sealed class RootDevelopmentPanelPart : DevelopmentPanelPart<RootScope>
{
    private readonly IProgress _progress;

    public RootDevelopmentPanelPart(IProgress progress, DevelopmentPanel panel) : base(panel) =>
        _progress = progress;

    [Button] private void ClearData() => PlayerPrefs.DeleteAll();
    [Button] private void Add5Cherries() => _progress.Persistant.AddCherries(5);
}
