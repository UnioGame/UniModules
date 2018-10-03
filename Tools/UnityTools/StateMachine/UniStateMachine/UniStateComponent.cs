using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Extensions;
using Modules.UnityToolsModule.Tools.UnityTools.Interfaces;
using StateMachine.ContextStateMachine;
using UnityEngine;

namespace UniStateMachine {
    
    public class UniStateComponent : MonoBehaviour,
        IContextStateBehaviour<IEnumerator>
    {
        
        protected List<IDisposable> _disposables = new List<IDisposable>();
        private IContext _contextProvider;
        private Lazy<IContextStateBehaviour<IEnumerator>> _stateBehaviour;
        	
        [SerializeField]
        private bool _isActive;

        #region public methods

        public IEnumerator Execute(IContext context) {
            _isActive = true;
            yield return _stateBehaviour.Value.Execute(context);
        }

        public void Exit(IContext context) {
            _isActive = false;
            _stateBehaviour.Value.Exit(context);
            _disposables.Cancel();
        }

        public void Dispose()
        {
            _stateBehaviour.Value.Dispose();
        }

        #endregion

        protected virtual IEnumerator ExecuteState(IContext contextProvider) {
            yield break;
        }

        protected virtual void OnEnter(IContext contextProvider) {
        }

        protected virtual void OnExit(IContext contextProvider) {
        }

        protected virtual void Awake() {
            _stateBehaviour = new Lazy<IContextStateBehaviour<IEnumerator>>(Create);
        }

        private IContextStateBehaviour<IEnumerator> Create() {
            
            var behaviour = new ProxyStateBehaviour();
            behaviour.Initialize(ExecuteState, OnEnter, OnExit);
            return behaviour;
        }

        protected virtual void OnDestroy() {
            _stateBehaviour.Value.Dispose();
        }
    }
}