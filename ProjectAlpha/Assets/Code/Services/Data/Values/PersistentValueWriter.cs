using System;

namespace Code.Services.Data
{
    public abstract class PersistentValueWriter<T> : IObservedValueWriter<T>
    {
        public event Action Changed;
        private readonly ObservedValue<T> _reader;

        protected readonly T DefaultValue;
        protected readonly string Key;
        private T _value;

        public T Value
        {
            get => _value;
            set
            {
                _value = value;
                Save(value);
                Changed?.Invoke();
            }
        }

        protected PersistentValueWriter(T defaultValue, string key)
        {
            _reader = new ObservedValue<T>(this);
            DefaultValue = defaultValue;
            Key = key;
        }

        public abstract void Restore();
        protected abstract void Save(T value);

        public static implicit operator ObservedValue<T>(PersistentValueWriter<T> observedBool) =>
            observedBool._reader;
    }
}
