using System;
using System.Threading;

namespace Code.Common
{
    public sealed class LinkedCancellationTokenSourceDisposable : IDisposable
    {
        private readonly CancellationTokenSource _cts;
        private readonly CancellationTokenSource _linkedCts;

        public CancellationToken Token => _linkedCts.Token;

        public LinkedCancellationTokenSourceDisposable(CancellationToken externalToken)
        {
            _cts = new CancellationTokenSource();
            _linkedCts = CancellationTokenSource.CreateLinkedTokenSource(_cts.Token, externalToken);
        }

        public void Dispose()
        {
            _cts.Cancel();
            _cts.Dispose();
        }
    }
}