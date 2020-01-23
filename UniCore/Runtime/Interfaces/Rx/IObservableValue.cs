namespace UniGreenModules.UniCore.Runtime.Interfaces.Rx
{
    using System;

    public interface IObservableValue<out T> : 
        IObservable<T>, 
        IDisposable,
        IReadonlyDataValue<T>, 
        IValueContainerStatus
    {
    }
}