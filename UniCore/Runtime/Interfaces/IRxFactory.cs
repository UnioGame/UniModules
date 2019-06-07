using System;

namespace UniGreenModules.UniCore.Runtime.Interfaces
{
    public interface IRxFactory<T>
    {

        IObservable<T> Create();

    }
}
