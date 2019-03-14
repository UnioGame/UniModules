using System;

namespace UniModule.UnityTools.Interfaces
{
    public interface IObservableDataValue<T> : 
        IObservable<T>, 
        IDisposable,
        IReadonlyDataValue<T>, 
        IDataValueParameters
    {
    }
}