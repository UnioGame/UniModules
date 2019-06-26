namespace UniGreenModules.UniCore.Runtime.Interfaces
{
    using System;

    public interface ITypeData : 
        IContextWriter,
        IValueContainerStatus, 
        IReadOnlyData, 
        ITypeDataObservable
    {
    }
}