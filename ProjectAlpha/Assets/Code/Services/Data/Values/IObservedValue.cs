using System;

namespace Code.Services.Data
{
// ReSharper disable once TypeParameterCanBeVariant
    public interface IObservedValue<T>
    {
        public event Action Changed;
        public T Value { get; }
    }
}