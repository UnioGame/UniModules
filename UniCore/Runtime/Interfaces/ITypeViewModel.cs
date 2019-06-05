namespace UniGreenModules.UniCore.Runtime.Interfaces
{
    using System;
    using DataFlow;
    using ObjectPool.Interfaces;

    public interface ITypeViewModel : IPoolable
    {
        /// <summary>
        /// is view model already initialized
        /// </summary>
        bool IsInitialized { get; }

        /// <summary>
        /// view lifetime
        /// </summary>
        ILifeTime LifeTime { get; }

        /// <summary>
        /// model type
        /// </summary>
        Type Type { get; }
    }
}