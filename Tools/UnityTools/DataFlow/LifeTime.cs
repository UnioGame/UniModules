using System;
using System.Collections.Generic;

namespace Assets.Modules.UnityToolsModule.Tools.UnityTools.DataFlow
{
    public class LifeTime
    {
        private List<IDisposable> disposables = new List<IDisposable>();
        private List<object> _referencies = new List<object>();
        private List<Action> _cleanupActions = new List<Action>();
        private bool _isTerminated;



        /// <summary>
        /// cleanup action, call when life time terminated
        /// </summary>
        public LifeTime AddCleanUpAction(Action f) {
            if (f == null)
                return this;
            _cleanupActions.Add(f);
            return this;
        }
    
        /// <summary>
        /// add child disposable object
        /// </summary>
        public LifeTime AddDispose(IDisposable item)
        {
            disposables.Add(item);
            return this;
        }

        /// <summary>
        /// save object from GC
        /// </summary>
        public LifeTime AddRef(object o) {
            _referencies.Add(o);
            return this;
        }

        public bool IsTerminated => _isTerminated;

        public void Release()
        {
            _isTerminated = true;
            for (int i = 0; i < disposables.Count; i++)
            {
                disposables[i]?.Dispose();
            }
            disposables.Clear();
            _referencies.Clear();
        }

    }
}
