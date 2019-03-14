using System;
using UniModule.UnityTools.Interfaces;
using UniRx;

namespace UniModule.UnityTools.Common
{
    public interface ITypeDataContainer : 
        ITypeDataWriter,
        IDataValueParameters
    {
        TData Get<TData>();

        bool Contains<TData>();

        bool Contains(Type type);
    }
}