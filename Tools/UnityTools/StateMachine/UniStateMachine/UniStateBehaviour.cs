using System;
using System.Collections;
using Assets.Scripts.Tools.StateMachine;
using Modules.UnityToolsModule.Tools.UnityTools.Interfaces;
using UniStateMachine;
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
        }

        protected virtual void OnExit()
        {
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
