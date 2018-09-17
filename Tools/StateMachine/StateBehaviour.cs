using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Extensions;

namespace Assets.Scripts.Tools.StateMachine
{
    public class StateBehaviour : IStateBehaviour<IEnumerator>
    {
        protected readonly List<IDisposable> _disposables = new List<IDisposable>();

        #region public methods

        public IEnumerator Execute()
        {

            Initialize();

            yield return ExecuteState();

        }

        public void Stop()
        {
            Dispose();
        }

        public virtual void Dispose()
        {
            _disposables.Cancel();
        }

        #endregion

        protected virtual void Initialize()
        {
        }

        protected virtual IEnumerator ExecuteState()
        {
            yield break;
        }

    }
}
