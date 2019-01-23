using System;
using System.Collections.Generic;
using Assets.Modules.UnityToolsModule.Tools.UnityTools.DataFlow;
using Assets.Tools.UnityTools.ObjectPool.Scripts;
using UnityTools.Common;

namespace Assets.Tools.UnityTools.Interfaces
{

    public interface IContextData<TContext> : IContextDataWriter<TContext>, ICopyableData<TContext>
    {
        IReadOnlyList<TContext> Contexts { get; }

        int Count { get; }
        
        bool HasValue(TContext context,Type type);
        
        bool HasValue<TValue>(TContext context);
             
        bool HasContext(TContext context);
        
        TData Get<TData>(TContext context);

        bool RemoveContext(TContext context);
        
        bool Remove<TData>(TContext context);

    }
}
