using System;

namespace UniModule.UnityTools.Common
{
    public interface ITypeDataWriter
    {
        bool Remove<TData>();
        bool Remove(Type type);
        void Add<TData>(TData data);
    }
}