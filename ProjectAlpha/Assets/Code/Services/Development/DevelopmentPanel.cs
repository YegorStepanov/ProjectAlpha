using System;
using Code.Scopes;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Code.Services.Development
{
    public sealed class DevelopmentPanel : MonoBehaviour
    {
        [ShowInInspector, HideInEditorMode, HideReferenceObjectPicker]
        private RootDevelopmentPanelPart _panelPart;

        [ShowInInspector, HideInEditorMode, HideReferenceObjectPicker]
        private DevelopmentMenuPanel _menuPanel;

        public void Bind<T>(DevelopmentPanelPart<T> part) where T : Scope
        {
            DevelopmentPanelPart<T> currentPart = GetPart<T>();
            AssertBindingIsCorrect(currentPart, part);
            SetPanel(part);
        }

        public void Unbind<T>(DevelopmentPanelPart<T> developmentPanelPart) where T : Scope
        {
            DevelopmentPanelPart<T> panelPart = GetPart<T>();
            AssertUnbindingIsCorrect(panelPart, developmentPanelPart);
            SetPanel<T>(null);
        }

        private DevelopmentPanelPart<T> GetPart<T>() where T : Scope => typeof(T) switch
        {
            Type t when t == typeof(RootScope) => _panelPart as DevelopmentPanelPart<T>,
            Type t when t == typeof(MenuScope) => _menuPanel as DevelopmentPanelPart<T>,
            Type t when t == typeof(BootstrapScope) => null,
            Type t when t == typeof(GameScope) => null,
            _ => throw new ArgumentOutOfRangeException(typeof(T).FullName)
            };

        private void SetPanel<T>(DevelopmentPanelPart<T> panel) where T : Scope
        {
            switch (typeof(T))
            {
                case Type t when t == typeof(RootScope):
                    _panelPart = panel as RootDevelopmentPanelPart;
                    break;
                case Type t when t == typeof(MenuScope):
                    _menuPanel = panel as DevelopmentMenuPanel;
                    break;
                case Type t when t == typeof(BootstrapScope):
                    break;
                case Type t when t == typeof(GameScope):
                    break;
                default:
                    throw new ArgumentOutOfRangeException(typeof(T).FullName);
            }
        }

        private void AssertBindingIsCorrect<T>(T currentPart, T part) where T : class
        {
            if (currentPart is not null)
                Debug.LogWarning($"{typeof(T).Name} is not null", this);

            if (currentPart == part)
                Debug.LogWarning($"Assigned the same object of {typeof(T).Name} type", this);
        }

        private void AssertUnbindingIsCorrect<T>(T currentPart, object part) where T : class
        {
            if (currentPart is null)
                Debug.LogWarning($"{typeof(T).Name} is null", this);

            if (currentPart != part)
                Debug.LogWarning($"Unassigned the different object of {typeof(T).Name} type", this);
        }
    }
}
