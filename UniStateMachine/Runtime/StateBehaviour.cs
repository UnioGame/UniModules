namespace UniGreenModules.UniStateMachine.Runtime
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Interfaces;
    using UniCore.Runtime.Extension;
    using UniGreenModules.UniCore.Runtime.Extension;
    using UniRx;

    public class StateBehaviour : IStateBehaviour<IEnumerator>
    {
        protected BoolReactiveProperty isActive = new BoolReactiveProperty();
        protected readonly List<IDisposable> _disposables = new List<IDisposable>();


        public IReadOnlyReactiveProperty<bool> IsActive => isActive;

        
        #region public methods

        public IEnumerator Execute()
        {

            if (isActive.Value)
                yield return ExecutionAwait();

            Initialize();
            
            isActive.Value = true;

            yield return ExecuteState();

            OnPostExecute();
        }

        public void Exit() {
            
            isActive.Value = false;
            _disposables.Cancel();
            OnExite();
            
        }



        #endregion

        protected virtual IEnumerator ExecutionAwait()
        {
            while (isActive.Value)
            {
                yield return null;
            }
        }
        
        protected virtual void OnExite()
        {
        }
        
        protected virtual void Initialize()
        {
        }

        protected virtual void OnPostExecute() {
            
        }

        protected virtual IEnumerator ExecuteState()
        {
            yield break;
        }

    }
}
