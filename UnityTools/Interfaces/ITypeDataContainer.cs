using System;
using UniModule.UnityTools.Interfaces;
using UniRx;

namespace UniModule.UnityTools.Common
{
    public interface ITypeDataContainer : 
        IContextWriter,
        IDataValueParameters
    {
        
        TData Get<TData>();

        bool Contains<TData>();
        
    }
}