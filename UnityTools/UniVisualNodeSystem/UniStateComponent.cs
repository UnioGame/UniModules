using System;
using System.Collections;
using System.Collections.Generic;
using UniModule.UnityTools.DataFlow;
using UniModule.UnityTools.Extension;
using UniModule.UnityTools.Interfaces;
using UniModule.UnityTools.UniStateMachine;
using UniModule.UnityTools.UniStateMachine.Interfaces;
using UnityEngine;

namespace UniStateMachine 
{
    
    public class UniStateComponent : MonoBehaviour,IContextState<IEnumerator>
    {
        protected List<IDisposable> _disposables = new List<IDisposable>();

        private IContextState<IEnumerator> _state;

        /// <summary>
        /// state local context data
        /// </summary>
        protected IContextData<IContext> _context;

        #region public methods

        public void Exit(IContext context)
        {

            var behaviour = GetBehaviour(context);
            behaviour.Exit(context);

        }

        public bool IsActive(IContext context)
        {
            var state = GetBehaviour(context);
            return state.IsActive(context);
        }

        public ILifeTime GetLifeTime(IContext context)
        {
            var state = GetBehaviour(context);
            return state.GetLifeTime(context);
        }

        /// <summary>
        /// stop ay execution of state
        /// release all resources
        /// </summary>
        public virtual void Dispose()
        {
            _disposables.Cancel();
            _state?.Dispose();
        }

        public IEnumerator Execute(IContext context)
        {
            StateLogger.LogState(string.Format("STATE EXECUTE {0} TYPE {1}", this.name, GetType().Name), this);
            var state = GetBehaviour(context);
            yield return state.Execute(context);
        }

        #endregion

        #region state behaviour methods

        protected void Initialize(IContextData<IContext> stateContext)
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

        protected virtual IContextState<IEnumerator> Create()
        {
            var behaviour = new ProxyStateBehaviour();
            behaviour.Initialize(ExecuteState, Initialize, OnExit);
            return behaviour;
        }

        protected virtual IContextState<IEnumerator> GetBehaviour(IContext context)
        {
            if (_state == null)
            {
                _state = Create();
            }

            return _state;
        }
    }
}