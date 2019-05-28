namespace UniGreenModules.UniNodeActors.Runtime.Interfaces
{
    using System;
    using UniCore.Runtime.DataFlow;
    using UniRx;

    public interface IActor : IDisposable
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