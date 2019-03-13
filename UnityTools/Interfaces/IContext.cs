using System;
using UniModule.UnityTools.DataFlow;
using UniModule.UnityTools.ObjectPool.Scripts;
using UniRx;

namespace UniModule.UnityTools.Interfaces
{
    public interface IContext : 
        IMessageBroker,
        IPoolable, IReadOnlyContext,
        IDisposable
    {
        
        /// <summary>
        /// remove data from context
        /// </summary>
        bool Remove<TData>();
        
        /// <summary>
        /// add data to context
        /// </summary>
        void Add<TData>(TData data);       

        
    }
}
