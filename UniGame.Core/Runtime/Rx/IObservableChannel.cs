namespace Taktika.Analytics.Runtime
{
    using System;

    public interface IObservableChannel<T> : IObservable<T>
    {
        void Publish(T message);
    }
}