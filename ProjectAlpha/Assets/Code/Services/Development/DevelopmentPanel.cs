using Sirenix.OdinInspector;
using UnityEngine;

namespace Code.Services.Development;

public sealed class DevelopmentPanel : MonoBehaviour
{
    [ShowInInspector, HideInEditorMode] private DevelopmentRootPanel _rootPanel;
    [ShowInInspector, HideInEditorMode] private DevelopmentMenuPanel _menuPanel;

    public void Bind(DevelopmentRootPanel rootPanel) =>
        BindPanel(ref rootPanel, ref _rootPanel);

    public void Bind(DevelopmentMenuPanel menuPanel) =>
        BindPanel(ref menuPanel, ref _menuPanel);

    public void Unbind(DevelopmentRootPanel rootPanel) =>
        UnbindPanel(ref rootPanel, ref _rootPanel);

    public void Unbind(DevelopmentMenuPanel menuPanel) =>
        UnbindPanel(ref menuPanel, ref _menuPanel);

    private void BindPanel<T>(ref T panel, ref T field) where T : class
    {
        if (field is not null)
            Debug.LogWarning($"{typeof(T).Name} is not null", this);

        field = panel;
    }

    private void UnbindPanel<T>(ref T panel, ref T field) where T : class
    {
        if (field is null)
            Debug.LogWarning($"{typeof(T).Name} is null", this);

        if (field == panel)
            Debug.LogWarning($"Assigned the same object of {typeof(T).Name} type", this);

        field = null;
    }
}
