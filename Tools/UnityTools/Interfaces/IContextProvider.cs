using System;
using Assets.Tools.Utils;

namespace Modules.UnityToolsModule.Tools.UnityTools.Interfaces
{

    public interface IContextProvider<TContext> : IPoolable
    {
        TData Get<TData>(TContext context, TData data);
        void RemoveContext(TContext context);
        void Remove<TData>(TContext context);
        void AddValue<TData>(TContext context, TData value);

    }
}
