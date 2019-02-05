using System;
using UniModule.UnityTools.Interfaces;

namespace UniModule.UnityTools.UniVisualNodeSystem.Connections
{
    
    public interface IBroadcastContextData<TContext> : IContextDataWriter<TContext>
    {
        
        void Add(IContextDataWriter<TContext> contextData);
        void Remove(IContextDataWriter<TContext> contextData);
        
    }
}