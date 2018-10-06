using System;
using System.Collections;
using Assets.Tools.UnityTools.Interfaces;
using Assets.Tools.UnityTools.StateMachine.Interfaces;
using UnityEngine;

namespace Assets.Tools.UnityTools.StateMachine.UniStateMachine
{
    [Serializable]
    public class UniStateBehaviour : ScriptableObject, IContextState<IEnumerator>
    {
        [NonSerialized]
        private IContextState<IEnumerator> _state;
        [NonSerialized]
        protected IContextProvider<IContext> _context;

        #region public methods

        public bool IsActive(IContext context)
        {
            var state = GetBehaviour();
            return state == null ? false : state.IsActive(context);
        }

        public void Exit(IContext context)
        {
            var behaviour = GetBehaviour();
            behaviour.Exit(context);
        }
        
        /// <summary>
        /// stop ay execution of state
        /// release all resources
        /// </summary>
        public virtual void Dispose()
        {
            _context?.Release();
            _state?.Dispose();
        }

        public IEnumerator Execute(IContext context)
        {
            StateLogger.LogState(string.Format("STATE EXECUTE {0} TYPE {1} CONTEXT {2}", 
                name, GetType().Name, context), this);

            var state = GetBehaviour();
            yield return state.Execute(context);
        }

        #endregion

        #region state behaviour methods

        protected void Initialize(IContextProvider<IContext> stateContext)
        {
            _context = stateContext;
            OnInitialize();
        }

        protected virtual void OnInitialize() { }

        protected virtual IEnumerator ExecuteState(IContext context)
        {
            yield break;
        }

        protected virtual void OnExit(IContext context) { }

        protected virtual void OnPostExecute(IContext context){}
        
        #endregion

        private IContextState<IEnumerator> Create()
        {
            var behaviour = new ProxyStateBehaviour();
            behaviour.Initialize(ExecuteState, Initialize, OnExit,OnPostExecute);
            return behaviour;
        }

        private IContextState<IEnumerator> GetBehaviour()
        { 
            if (_state == null)
            {
                _state = Create();
            }
            return _state;
        }

    }
}
