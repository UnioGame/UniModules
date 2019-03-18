using System;
using UniModule.UnityTools.Interfaces;
using UniRx;

namespace UniModule.UnityTools.Common
{
    public interface ITypeData : 
        IContextWriter,
        IDataValueParameters
    {
        
        TData Get<TData>();

        bool Contains<TData>();
        
    }
}