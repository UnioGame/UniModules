namespace UniModules.UniActors.Runtime.Interfaces
{
    using UniCore.Runtime.DataFlow;
    using UniCore.Runtime.DataFlow.Interfaces;
    using global::UniGame.Core.Runtime.ObjectPool;
    using global::UniGame.Core.Runtime;
    using UniRx;

    public interface IActor : IPoolable, IProcess
    {
        
        /// <summary>
        /// Actor context data
        /// </summary>
        IMessageBroker MessageBroker { get; }

        /// <summary>
        /// Actor life time object
        /// </summary>
        ILifeTime LifeTime { get; }

    }
}