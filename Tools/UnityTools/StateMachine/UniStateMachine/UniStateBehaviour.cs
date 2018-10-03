using System;
using System.Collections;
using Assets.Tools.UnityTools.Common;
using Assets.Tools.UnityTools.Interfaces;
using Assets.Tools.UnityTools.ScriptableObjects;
using Assets.Tools.UnityTools.StateMachine.Interfaces;
using UnityEngine;

namespace Assets.Tools.UnityTools.StateMachine.UniStateMachine
{
    [Serializable]
    public class UniStateBehaviour : 
        ScriptableObjectRoutine<IContext>,
        IContextStateBehaviour<IEnumerator>
    {
        [NonSerialized]
        protected IContextProvider<IContext> _stateContext;

        [SerializeField]
        private bool _isActive;
 
        #region public methods

        public void Exit(IContext context)
        {
            var behaviour = GetBehaviour(context);
            behaviour.Exit(context);

            _stateContext.Remove<IContextStateBehaviour<IEnumerator>>(context);
        }
        
        public void Dispose()
        {
            _stateContext.Release();
        }

        #endregion

        protected sealed override IEnumerator OnExecute(IContext context)
        {
            
            StateLogger.LogState(string.Format("STATE EXECUTE {0} TYPE {1}",this.name,GetType().Name),this);
            yield return GetBehaviour(context).Execute(context);
            
        }

        protected override void OnInitialize()
        {
            _stateContext = new ContextProviderProvider<IContext>();
            base.OnInitialize();
        }

        protected virtual IEnumerator ExecuteState(IContext context)
        {
            yield break;
        }

        protected virtual void OnEnter(IContext context) 
        {
            _isActive = true;
        }

        protected virtual void OnExit(IContext context)
        {
            _isActive = false;
        }

        protected virtual IContextStateBehaviour<IEnumerator> Create()
        {
            var behaviour = new ProxyStateBehaviour();
            behaviour.Initialize(ExecuteState, OnEnter, OnExit);
            return behaviour;
        }

        protected IContextStateBehaviour<IEnumerator> GetBehaviour(IContext context)
        {

            var contextState = _stateContext.Get<IContextStateBehaviour<IEnumerator>>(context);
            if (contextState == null)
            {
                contextState = Create();
                _stateContext.AddValue(context, contextState);
            }

            return contextState;
        }

    }
}
