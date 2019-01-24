using System;
using System.Collections.Generic;
using Assets.Tools.UnityTools.StateMachine.Interfaces;
using UniModule.UnityTools.Extension;
using UniRx.Async;

namespace Assets.Tools.UnityTools.StateMachine.AsyncStateMachine
{

    public class AsyncStateBehaviour : IStateBehaviour<UniTask>
    {
        protected readonly List<IDisposable> _disposables = new List<IDisposable>();

        public async UniTask Execute()
        {

            if (IsActive)
                await OnAlreadyActive();
            
            IsActive = true;
            
            Initialize();

            await ExecuteState();

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

        protected virtual async UniTask ExecuteState()
        {
            
        }

        protected virtual async UniTask OnAlreadyActive()
        {
            while (IsActive)
            {
                await UniTask.Yield(PlayerLoopTiming.Update);
            }
        }

    }
}
