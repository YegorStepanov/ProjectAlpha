using System;
using Code.Services.Data;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Code.Services.Development;

public sealed class DevelopmentRootPanel : IDisposable
{
    private readonly IProgress _progress;
    private readonly DevelopmentPanel _panel;

    public DevelopmentRootPanel(DevelopmentPanel panel, IProgress progress)
    {
        _progress = progress;
        _panel = panel;
        _panel.Bind(this);
    }

    public void Dispose() =>
        _panel.Unbind(this);

    [Button]
    public void ClearData() => PlayerPrefs.DeleteAll();

    [Button]
    public void Add5Cherries() => _progress.Persistant.AddCherries(5);
}
