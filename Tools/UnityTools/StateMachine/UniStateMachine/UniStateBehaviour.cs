﻿using System;
using System.Collections;
using Assets.Scripts.Tools.StateMachine;
using Modules.UnityToolsModule.Tools.UnityTools.Interfaces;
using UnityEngine;

namespace UniStateMachine
{
    [Serializable]
    public class UniStateBehaviour : ScriptableObjectRoutine<IEnumerator>, IStateBehaviour<IContextProvider,IEnumerator>
    {

        [NonSerialized]
        protected IContextProvider _contextProvider;
        [NonSerialized]
        private Lazy<IStateBehaviour<IEnumerator>> _stateBehaviour;

        [SerializeField]
        private bool _isActive;
        
        public bool IsActive => _stateBehaviour.Value.IsActive;

        #region public methods
        
        public void Initialize(IContextProvider contextProvider)
        {
            _contextProvider = contextProvider;
            OnContextChanged(_contextProvider);
        }
        
        public void Exit()
        {
            _stateBehaviour.Value.Exit();
        }

        #endregion
        
        protected override IEnumerator OnExecute()
        {
            
            StateLogger.LogState(string.Format("STATE EXECUTE {0} TYPE {1}",this.name,GetType().Name),this);
            yield return _stateBehaviour.Value.Execute();
            
        }

        protected override void OnInitialize()
        {
            if(_stateBehaviour == null)
                _stateBehaviour = new Lazy<IStateBehaviour<IEnumerator>>(Create);
            base.OnInitialize();
        }

        protected virtual IEnumerator ExecuteState()
        {
            yield break;
        }

        protected virtual void OnEnter() 
        {
            _isActive = true;
        }

        protected virtual void OnExit()
        {
            _isActive = false;
        }

        protected virtual void OnContextChanged(IContextProvider contextProvider)
        {
            
        }

        protected virtual IStateBehaviour<IEnumerator> Create()
        {
            var behaviour = new ProxyStateBehaviour();
            behaviour.Initialize(ExecuteState, OnEnter, OnExit);
            return behaviour;
        }

    }
}
