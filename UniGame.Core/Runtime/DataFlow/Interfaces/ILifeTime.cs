namespace UniModules.UniGame.Core.Runtime.DataFlow.Interfaces
{
    using System;

    public interface ILifeTime
    {
        /// <summary>
        /// cleanup action, call when life time terminated
        /// </summary>
        ILifeTime AddCleanUpAction(Action cleanAction);

        /// <summary>
        /// add child disposable object
        /// </summary>
        ILifeTime AddDispose(IDisposable item);

        /// <summary>
        /// save object from GC
        /// </summary>
        ILifeTime AddRef(object o);

        /// <summary>
        /// is lifetime termicated
        /// </summary>
        bool IsTerminated { get; }
    }
}