using System;

namespace UniGreenModules.UniCore.Runtime.Interfaces
{
    public interface IObservableFactory<T>
    {

        IObservable<T> Create();

    }
}
