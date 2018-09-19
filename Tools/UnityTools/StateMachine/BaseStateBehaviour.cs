using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Extensions;

namespace Assets.Scripts.Tools.StateMachine {
    public class BaseStateBehaviour : IStateBehaviour<IEnumerator> {
        protected readonly List<IDisposable> _disposables = new List<IDisposable>();
        protected bool _isActive;

        public IEnumerator Execute() {
            
            Exit();
            
            _isActive = true;
            
            Initialize();

            yield return ExecuteState();

        }

        public void Exit() {
            
            _isActive = false;
            _disposables.Cancel();
            OnStateStop();
            
        }

        protected virtual void OnStateStop()
        {
        }

        protected virtual void Initialize()
        {
        }

        protected virtual IEnumerator ExecuteState()
        {
            yield break;
        }
    }
}