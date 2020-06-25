namespace UniGreenModules.UniCore.Runtime.DataFlow
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using Interfaces;
    using ObjectPool.Runtime.Interfaces;
    using Rx.Extensions;
    using UniModules.UniGame.Core.Runtime.DataFlow.Interfaces;

    public class LifeTime : ILifeTime, IPoolable
    {
        public readonly static ILifeTime TerminatedLifetime;
        
        private List<IDisposable> disposables = new List<IDisposable>();
        private List<object> referencies = new List<object>();
        private List<Action> cleanupActions = new List<Action>();
        public bool isTerminated;

        static LifeTime()
        {
            var completedLifetime = new LifeTime();
            completedLifetime.Release();
            TerminatedLifetime = completedLifetime;
        }
        
        /// <summary>
        /// cleanup action, call when life time terminated
        /// </summary>
        public ILifeTime AddCleanUpAction(Action cleanAction) 
        {
            if (cleanAction == null)
                return this;
            //call cleanup immediate. lite time already ended
            if (isTerminated) {
                cleanAction?.Invoke();
                return this;
            }
            cleanupActions.Add(cleanAction);
            return this;
        }
    
        /// <summary>
        /// add child disposable object
        /// </summary>
        public ILifeTime AddDispose(IDisposable item)
        {
            if (isTerminated) {
                item.Cancel();
                return this;
            }
            
            disposables.Add(item);
            return this;
        }

        /// <summary>
        /// save object from GC
        /// </summary>
        public ILifeTime AddRef(object o)
        {
            if (isTerminated)
                return this;
            referencies.Add(o);
            return this;
        }

        /// <summary>
        /// is lifetime terminated
        /// </summary>
        public bool IsTerminated => isTerminated;

        /// <summary>
        /// restart lifetime
        /// </summary>
        public void Restart()
        {
            Release();
            isTerminated = false;
        }
        
        /// <summary>
        /// invoke all cleanup actions
        /// </summary>
        public void Release()
        {
            if (isTerminated)
                return;
            
            isTerminated = true;
            
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

                
        #region type convertion

        public static implicit operator CancellationTokenSource(LifeTime lifeTime)
        {
            var tokenSource = new CancellationTokenSource();
            lifeTime.AddDispose(tokenSource);
            return tokenSource;
        } 

        #endregion
    }
}
