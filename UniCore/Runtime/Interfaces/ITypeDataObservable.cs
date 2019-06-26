namespace UniGreenModules.UniCore.Runtime.Interfaces
{
    using System;

    public interface ITypeDataObservable
    {
        
        IObservable<T> GetObservable<T>();
        
    }
}