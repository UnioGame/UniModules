using System;
using Assets.Modules.UnityToolsModule.Tools.UnityTools.DataFlow;
using Assets.Tools.UnityTools.ObjectPool.Scripts;
using UniRx;

namespace Assets.Tools.UnityTools.Interfaces
{
    public interface IContext : IMessageBroker,IPoolable
    {
        
        /// <summary>
        /// lifetime of this context
        /// </summary>
        ILifeTime LifeTime { get; }
        
        /// <summary>
        /// get data from context object
        /// </summary>
        TData Get<TData>();
        
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
