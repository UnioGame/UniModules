using System;
using System.Collections;
using System.Collections.Generic;
using Modules.UnityToolsModule.Tools.UnityTools.Interfaces;
using StateMachine.ContextStateMachine;
using UnityEngine;

namespace UniStateMachine
{
    [Serializable]
    public class UniStateBehaviour : 
        ScriptableObjectRoutine<IContext>,
        IContextStateBehaviour<IEnumerator>
    {

        [NonSerialized]
        private Lazy<IContextStateBehaviour<IEnumerator>> _stateBehaviour;

        [SerializeField]
        private bool _isActive;
 
        #region public methods

        public void Exit(IContext contextProvider)
        {
            _stateBehaviour.Value.Exit(contextProvider);
        }


        public void Dispose()
        {
            _stateBehaviour.Value.Dispose();
        }

        #endregion

        protected override IEnumerator OnExecute(IContext context)
        {
            
            StateLogger.LogState(string.Format("STATE EXECUTE {0} TYPE {1}",this.name,GetType().Name),this);
            yield return _stateBehaviour.Value.Execute(context);
            
        }

        protected override void OnInitialize()
        {
            if(_stateBehaviour == null)
                _stateBehaviour = new Lazy<IContextStateBehaviour<IEnumerator>>(Create);
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

    }
}
