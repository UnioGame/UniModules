using System;
using UniModule.UnityTools.Common;

namespace UniStateMachine.Nodes
{
    public interface ITypeValueObservable
    {
        IObservable<TypeDataChanged> UpdateValueObservable { get; }
        IObservable<TypeDataChanged> DataRemoveObservable { get; }
        IObservable<ITypeData> EmptyDataObservable { get; }
    }
}