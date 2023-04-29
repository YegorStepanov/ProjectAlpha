using System;

namespace Code.Services.Data
{
    public sealed class ObservedValueWriter<T> : IObservedValueWriter<T>
    {
        public event Action Changed;
        private readonly ObservedValue<T> _reader;
        private T _value;

        public T Value
        {
            get => _value;
            set
            {
                _value = value;
                Changed?.Invoke();
            }
        }

        public ObservedValueWriter(T defaultValue)
        {
            _reader = new ObservedValue<T>(this);
            Value = defaultValue;
        }

        public static implicit operator ObservedValue<T>(ObservedValueWriter<T> writer) =>
            writer._reader;
    }
}