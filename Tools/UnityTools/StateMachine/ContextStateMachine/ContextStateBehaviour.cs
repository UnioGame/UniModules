using System;
using System.Collections;
using Assets.Tools.UnityTools.Common;
using Assets.Tools.UnityTools.Interfaces;
using Assets.Tools.UnityTools.StateMachine.Interfaces;

namespace Assets.Tools.UnityTools.StateMachine.ContextStateMachine
{
    public abstract class ContextStateBehaviour : IContextStateBehaviour<IEnumerator>
    {

        private bool _initialized = false;

        /// <summary>
        /// state local context data
        /// </summary>
        protected IContextProvider<IContext> _context;

        #region public methods

        public IEnumerator Execute(IContext context)
        {
            if (_initialized == false)
            {
                _initialized = true;
                _context = new ContextProviderProvider<IContext>();
                OnInitialize(_context);
            }

            yield return ExecuteState(context);

            OnPostExecute(context);
        }

        public void Exit(IContext context)
        {
            OnExit(context);
            //remove all local state data
            _context?.RemoveContext(context);
        }

        public virtual void Dispose()
        {
            _context?.Release();
        }

        public bool IsActive(IContext context)
        {
            return _context?.HasContext(context) ?? false;
        }

        #endregion

        protected virtual void OnInitialize(IContextProvider<IContext> stateContext) { }

        protected virtual void OnExit(IContext context){}

        protected virtual void OnPostExecute(IContext context){}

        protected abstract IEnumerator ExecuteState(IContext context);

    }
}
