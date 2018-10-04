using System;
using System.Collections;
using Assets.Tools.UnityTools.Common;
using Assets.Tools.UnityTools.Interfaces;
using Assets.Tools.UnityTools.StateMachine.Interfaces;
using UnityEngine;

namespace Assets.Tools.UnityTools.StateMachine.UniStateMachine
{
    [Serializable]
    public class UniStateBehaviour : ScriptableObject, IContextStateBehaviour<IEnumerator>
    {
        [NonSerialized]
        private IContextStateBehaviour<IEnumerator> _state;

        /// <summary>
        /// state local context data
        /// </summary>
        [NonSerialized]
        protected IContextProvider<IContext> _context;

        #region public methods

        public void Exit(IContext context)
        {
            var behaviour = GetBehaviour(context);
            behaviour.Exit(context);

            //remove all local state data
            _context.RemoveContext(context);
        }
        
        /// <summary>
        /// stop ay execution of state
        /// release all resources
        /// </summary>
        public virtual void Dispose()
        {
            _state?.Dispose();
            _context?.Release();
        }

        public IEnumerator Execute(IContext context)
        {
            if (_context == null)
            {
                _context = new ContextProviderProvider<IContext>();
            }
            var state = GetBehaviour(context);

            StateLogger.LogState(string.Format("STATE EXECUTE {0} FROM {1} TYPE {2}", state, this.name, GetType().Name), this);
            yield return state.Execute(context);
        }

        #endregion

        #region state behaviour methods

        protected virtual void OnInitialize(){}

        protected virtual IEnumerator ExecuteState(IContext context)
        {
            yield break;
        }

        protected virtual void OnExit(IContext context) { }

        #endregion

        protected virtual IContextStateBehaviour<IEnumerator> Create()
        {
            var behaviour = new ProxyStateBehaviour();
            behaviour.Initialize(ExecuteState, OnInitialize, OnExit);
            return behaviour;
        }

        protected virtual IContextStateBehaviour<IEnumerator> GetBehaviour(IContext context)
        { 
            if (_state == null)
            {
                _state = Create();
            }

            return _state;
        }

    }
}
