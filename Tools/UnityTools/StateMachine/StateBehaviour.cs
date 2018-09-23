using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Extensions;
using UniStateMachine;

namespace UniStateMachine
{
    public class StateBehaviour : IStateBehaviour<IEnumerator>
    {
        protected readonly List<IDisposable> _disposables = new List<IDisposable>();


        public bool IsActive { get; protected set; }

        
        #region public methods

        public IEnumerator Execute()
        {

            if (IsActive)
                yield return Wait();
            
            IsActive = true;
            
            Initialize();

            yield return ExecuteState();

            OnFinish();
        }

        public void Exit() {
            
            IsActive = false;
            _disposables.Cancel();
            OnStateStop();
            
        }



        #endregion

        protected virtual IEnumerator Wait()
        {
            while (IsActive)
            {
                yield return null;
            }
        }
        
        protected virtual void OnStateStop()
        {
        }
        
        protected virtual void Initialize()
        {
        }

        protected virtual void OnFinish() {
            
        }

        protected virtual IEnumerator ExecuteState()
        {
            yield break;
        }

    }
}
