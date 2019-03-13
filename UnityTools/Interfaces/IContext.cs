using System;
using UniModule.UnityTools.DataFlow;
using UniModule.UnityTools.ObjectPool.Scripts;
using UniModule.UnityTools.UniStateMachine.Interfaces;
using UniRx;

namespace UniModule.UnityTools.Interfaces
{
    public interface IContext : 
        IMessageBroker,
        IPoolable, 
        IReadOnlyContext,
        IDisposable,
        ILifeTimeContext
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
