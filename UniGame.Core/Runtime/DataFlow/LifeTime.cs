namespace UniGreenModules.UniCore.Runtime.DataFlow
{
    using System;
    using System.Collections.Generic;
    using Interfaces;
    using ObjectPool.Runtime.Interfaces;
    using Rx.Extensions;
    using UnityEngine.Profiling;

    public class LifeTime : ILifeTime, IPoolable
    {
        private List<IDisposable> disposables = new List<IDisposable>();
        private List<object> referencies = new List<object>();
        private List<Action> cleanupActions = new List<Action>();
        private bool _isTerminated;

        /// <summary>
        /// cleanup action, call when life time terminated
        /// </summary>
        public ILifeTime AddCleanUpAction(Action cleanAction) 
        {
            if (cleanAction == null)
                return this;

            cleanupActions.Add(cleanAction);
            return this;
        }
    
        /// <summary>
        /// add child disposable object
        /// </summary>
        public ILifeTime AddDispose(IDisposable item)
        {
            disposables.Add(item);
            return this;
        }

        /// <summary>
        /// save object from GC
        /// </summary>
        public ILifeTime AddRef(object o) {
            referencies.Add(o);
            return this;
        }

        /// <summary>
        /// is lifetime terminated
        /// </summary>
        public bool IsTerminated => _isTerminated;

        /// <summary>
        /// restart lifetime
        /// </summary>
        public void Restart()
        {
            Release();
            _isTerminated = false;
        }
        
        /// <summary>
        /// invoke all cleanup actions
        /// </summary>
        public void Release()
        {
            _isTerminated = true;
            
            for (var i = cleanupActions.Count-1; i >= 0; i--)
            {
                cleanupActions[i]?.Invoke();
            }
            
            for (var i = disposables.Count-1; i >= 0; i--) {
                disposables[i].Cancel();
            }

            cleanupActions.Clear();
            disposables.Clear();
            referencies.Clear();
        }

    }
}
