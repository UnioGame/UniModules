using System;

namespace Assets.Modules.UnityToolsModule.Tools.UnityTools.DataFlow
{
    public interface ILifeTime
    {
        /// <summary>
        /// cleanup action, call when life time terminated
        /// </summary>
        LifeTime AddCleanUpAction(Action cleanAction);

        /// <summary>
        /// add child disposable object
        /// </summary>
        LifeTime AddDispose(IDisposable item);

        /// <summary>
        /// save object from GC
        /// </summary>
        LifeTime AddRef(object o);

        /// <summary>
        /// is lifetime termicated
        /// </summary>
        bool IsTerminated { get; }
    }
}