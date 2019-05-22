using System;

namespace UniStateMachine.Nodes
{
    using UniGreenModules.UniCore.Runtime.Interfaces;

    public interface ITypeValueObservable
    {
        IObservable<TypeDataChanged> UpdateValueObservable { get; }
        IObservable<TypeDataChanged> DataRemoveObservable { get; }
        IObservable<ITypeData> EmptyDataObservable { get; }
    }
}