using System.Collections.Generic;
using Assets.Modules.UnityToolsModule.Tools.UnityTools.DataFlow;
using Assets.Tools.UnityTools.ObjectPool.Scripts;

namespace Assets.Tools.UnityTools.Interfaces
{

    public interface IContextData<TContext>
    {
        IReadOnlyCollection<TContext> Contexts { get; }

        TData Get<TData>(TContext context);

        bool RemoveContext(TContext context);
        
        bool Remove<TData>(TContext context);
        
        bool AddValue<TData>(TContext context, TData value);
        
        bool HasContext(TContext context);

    }
}
