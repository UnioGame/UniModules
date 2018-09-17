using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Extensions;
using Assets.Scripts.Tools.StateMachine;
using UniRx.Async;
using UnityEngine;


namespace Assets.Scripts.Tools.StateMachine
{

    public class AsyncStateBehaviour : IStateBehaviour<UniTask>
    {
        protected readonly List<IDisposable> _disposables = new List<IDisposable>();

        #region public methods

        /// <summary>
        /// TODO add cancelation token
        /// </summary>
        /// <returns></returns>
        public async UniTask Execute()
        {
            Initialize();

            await ExecuteState();
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

        protected virtual async UniTask ExecuteState()
        {
            
        }

    }
}
