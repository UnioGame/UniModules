namespace UniGreenModules.UniNodeSystem.Runtime.Interfaces
{
    using System;
    using UniCore.Runtime.Interfaces;

    public interface ITypeValueObservable
    {
        IObservable<TypeDataChanged> UpdateValueObservable { get; }
        IObservable<TypeDataChanged> DataRemoveObservable { get; }
        IObservable<ITypeData> EmptyDataObservable { get; }
    }
}