using System;

namespace Code.Services.Data
{
    public sealed class ObservedValue<T> : IObservedValue<T>
    {
        private readonly IObservedValueWriter<T> _writer;

        public T Value => _writer.Value;

        public event Action Changed
        {
            add => _writer.Changed += value;
            remove => _writer.Changed -= value;
        }

        public ObservedValue(IObservedValueWriter<T> writer) =>
            _writer = writer;

        public override string ToString() =>
            Value.ToString();

        public static implicit operator T(ObservedValue<T> writer) =>
            writer.Value;
    }
}
