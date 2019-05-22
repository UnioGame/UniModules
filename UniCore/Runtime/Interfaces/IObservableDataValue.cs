namespace UniGreenModules.UniCore.Runtime.Interfaces
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