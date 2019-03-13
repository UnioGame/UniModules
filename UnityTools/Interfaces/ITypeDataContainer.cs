using System;

namespace UniModule.UnityTools.Common
{
    public interface ITypeDataContainer : ITypeDataWriter
    {
        TData Get<TData>();

        bool Contains<TData>();

        bool Contains(Type type);
    }
}