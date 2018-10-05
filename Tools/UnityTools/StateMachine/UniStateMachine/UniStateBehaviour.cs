using System;
using System.Collections;
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
        [NonSerialized]
        protected IContextProvider<IContext> _context;

        #region public methods

        public bool IsActive(IContext context)
        {
            var state = GetBehaviour(context);
            return state.IsActive(context);
        }

        public void Exit(IContext context)
        {
            var behaviour = GetBehaviour(context);
            behaviour.Exit(context);
        }
        
        /// <summary>
        /// stop ay execution of state
        /// release all resources
        /// </summary>
        public virtual void Dispose()
        {
            _state?.Dispose();
        }

        public IEnumerator Execute(IContext context)
        {
            var state = GetBehaviour(context);

            StateLogger.LogState(string.Format("STATE EXECUTE {0} FROM {1} TYPE {2}", state, this.name, GetType().Name), this);
            yield return state.Execute(context);
        }

        #endregion

        #region state behaviour methods

        protected void Initialize(IContextProvider<IContext> stateContext)
        {
            _context = stateContext;
            OnInitialize();
        }

        protected virtual void OnInitialize()
        {

        }

        protected virtual IEnumerator ExecuteState(IContext context)
        {
            yield break;
        }

        protected virtual void OnExit(IContext context) { }

        #endregion

        protected virtual IContextStateBehaviour<IEnumerator> Create()
        {
            var behaviour = new ProxyStateBehaviour();
            behaviour.Initialize(ExecuteState, Initialize, OnExit);
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
