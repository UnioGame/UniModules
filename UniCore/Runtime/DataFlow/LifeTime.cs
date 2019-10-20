namespace UniGreenModules.UniCore.Runtime.DataFlow
{
    using System;
    using System.Collections.Generic;
    using ObjectPool.Interfaces;
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
            Profiler.BeginSample("AddCleanUpAction");
            cleanupActions.Add(cleanAction);
            Profiler.EndSample();
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
            
            for (int i = cleanupActions.Count-1; i >= 0; i--)
            {
                cleanupActions[i]();
            }
            
            for (int i = disposables.Count-1; i >= 0; i--)
            {
                disposables[i].Dispose();
            }

            cleanupActions.Clear();
            disposables.Clear();
            referencies.Clear();
        }

    }
}
