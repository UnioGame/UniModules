using System;

namespace UniModule.UnityTools.Common
{
    public interface IContextWriter
    {
        bool Remove<TData>();

        void Add<TData>(TData data);
    }
}