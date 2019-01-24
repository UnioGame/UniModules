using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Tools.UnityTools.StateMachine.Interfaces;
using UniModule.UnityTools.Extension;

namespace Assets.Tools.UnityTools.StateMachine
{
    
    public class BaseStateBehaviour : IStateBehaviour<IEnumerator> {
        
        protected readonly List<IDisposable> _disposables = new List<IDisposable>();

        public IEnumerator Execute()
        {

            if (IsActive)
                yield return OnAlreadyActive();
            
            IsActive = true;
            
            Initialize();

            yield return ExecuteState();

        }

        public bool IsActive { get; protected set; }

        public void Exit() {
            
            IsActive = false;
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

        protected virtual IEnumerator OnAlreadyActive()
        {
            while (IsActive)
            {
                yield return null;
            }
        }
    }
}