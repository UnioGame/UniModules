using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Extensions;

namespace Assets.Scripts.Tools.StateMachine
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

        }

        public void Stop() {
            
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

        protected virtual IEnumerator ExecuteState()
        {
            yield break;
        }

    }
}
