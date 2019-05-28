namespace UniGreenModules.UniNodeActors.Runtime.Interfaces
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UniCore.Runtime.DataFlow;
    using UniCore.Runtime.Interfaces;
    using UniCore.Runtime.ObjectPool.Interfaces;
    using UniModule.UnityTools.UniStateMachine.Interfaces;
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