namespace UniGreenModules.UniCore.Runtime.Interfaces.Rx
{
    using System;

    public interface IObservableDataValue<T> : 
        IObservable<T>, 
        IDisposable,
        IReadonlyDataValue<T>, 
        IDataValueParameters
    {
    }
}