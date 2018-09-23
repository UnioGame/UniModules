using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Extensions;
using UnityEngine;

namespace UniStateMachine {
    
    public class UniStateComponent : MonoBehaviour, IStateBehaviour<IContextProvider,IEnumerator> {
        
        protected List<IDisposable> _disposables = new List<IDisposable>();
        private IContextProvider _contextProvider;
        private Lazy<IStateBehaviour<IEnumerator>> _stateBehaviour;
        private WeakReference<IContextProvider> _context;
        	
        [SerializeField]
        private bool _isActive;
        
        public bool IsActive => _stateBehaviour.Value.IsActive;

        #region public methods

        public void Initialize(IContextProvider contextProvider) {
            
            _contextProvider = contextProvider;
            
        }

        public IEnumerator Execute() {
            _isActive = true;
            yield return _stateBehaviour.Value.Execute();
        }

        public void Exit() {
            _isActive = false;
            _stateBehaviour.Value.Exit();
            _disposables.Cancel();
        }

        #endregion

        protected virtual IEnumerator ExecuteState() {
            yield break;
        }


        protected virtual void OnEnter() {
        }

        protected virtual void OnExit() {
        }

        protected virtual void Awake() {
            _stateBehaviour = new Lazy<IStateBehaviour<IEnumerator>>(Create);
        }

        private IStateBehaviour<IEnumerator> Create() {
            
            var behaviour = new ProxyStateBehaviour();
            behaviour.Initialize(ExecuteState, OnEnter, OnExit);
            return behaviour;
        }

        protected virtual void OnDestroy() {
            if (IsActive) {
                Debug.LogErrorFormat("Destroy Active state {0} {1}",this.name,this.GetType().Name);
                Exit();
            }
        }
    }
}