using System;
using System.Collections;
using Assets.Scripts.Tools.StateMachine;
using UniStateMachine;
using UnityEngine;

namespace UniStateMachine
{
    [Serializable]
    public class UniStateBehaviour : ScriptableObject, IStateBehaviour<IContextProvider,IEnumerator>
    {

        [NonSerialized]
        private IContextProvider _contextProvider;
        [NonSerialized]
        private Lazy<IStateBehaviour<IEnumerator>> _stateBehaviour;
        [NonSerialized]
        private WeakReference<IContextProvider> _context;
        
        public bool IsActive => _stateBehaviour.Value.IsActive;

        #region public methods
        
        public void Initialize(IContextProvider contextProvider)
        {
            
            if(_stateBehaviour == null)
                _stateBehaviour = new Lazy<IStateBehaviour<IEnumerator>>(Create);
            _contextProvider = contextProvider;
            
        }
        
        public IEnumerator Execute()
        {
            yield return _stateBehaviour.Value.Execute();
        }

        public void Exit()
        {
            _stateBehaviour.Value.Exit();
        }

        #endregion
        
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

        private IStateBehaviour<IEnumerator> Create()
        {
            var behaviour = new ProxyStateBehaviour();
            behaviour.Initialize(ExecuteState, OnEnter, OnExit);
            return behaviour;
        }

    }
}
