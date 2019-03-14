using System;
using UniModule.UnityTools.Common;

namespace UniStateMachine.Nodes
{
    public interface ITypeValueObservable
    {
        IObservable<TypeValueUnit> UpdateValueObservable { get; }
        IObservable<TypeValueUnit> DataRemoveObservable { get; }
        IObservable<ITypeDataContainer> EmptyDataObservable { get; }
    }
}