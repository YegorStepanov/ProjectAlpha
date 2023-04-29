using System;
using Code.Scopes;

namespace Code.Services.Development
{
    public abstract class DevelopmentPanelPart<T> : IDisposable where T : Scope
    {
        private readonly DevelopmentPanel _panel;

        protected DevelopmentPanelPart(DevelopmentPanel panel)
        {
            _panel = panel;
            _panel.Bind(this);
        }

        void IDisposable.Dispose() =>
            _panel.Unbind(this);
    }
}