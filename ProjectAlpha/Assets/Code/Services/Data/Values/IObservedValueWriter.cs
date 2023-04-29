using System;

namespace Code.Services.Data
{
    public interface IObservedValueWriter<T>
    {
        public event Action Changed;
        public T Value { get; set; }
    }
}