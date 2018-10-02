using System;
using System.Collections.Generic;
using Assets.Scripts.Extensions;
using Modules.UnityToolsModule.Tools.UnityTools.Interfaces;
using StateMachine.ContextStateMachine;

namespace UniStateMachine
{
    public class ContextStateMachine<TAwaiter> : 
        IContextStateMachine<TAwaiter>
    {
            
        private readonly IRoutineExecutor<TAwaiter> _stateExecutor;

        protected List<IDisposable> disposables;
        protected IContextProvider contextProvider;

        #region public methods

        public ContextStateMachine(IRoutineExecutor<TAwaiter> stateExecutor)
        {
            disposables = new List<IDisposable>();
            _stateExecutor = stateExecutor;
        }

        public virtual void Dispose() {

            StopActiveState();
            disposables.Cancel();
            
        }

        public void Execute(IContextStateBehaviour<TAwaiter> state, IContextProvider context)
        {
            ChangeState(state, context);
        }

        public virtual void Stop()
        {
            StopActiveState();
        }

        #endregion

        #region public properties

        public IContextStateBehaviour<TAwaiter> ActiveState { get; protected set; }

        public bool IsActive => _stateExecutor.IsActive;

        #endregion

        private void ChangeState(IContextStateBehaviour<TAwaiter> state, 
            IContextProvider context)
        {

            StopActiveState();

            if (state == null)
            {
                ActiveState = null;
                contextProvider = null;
                return;
            }

            ActiveState = state;
            contextProvider = context;

            StartState(state, context);
            
        }

        private void StartState(IContextStateBehaviour<TAwaiter> state, IContextProvider context)
        {

            var routine = state.Execute(context);
            _stateExecutor.Execute(routine);
            
        }

        private void StopActiveState()
        {
            _stateExecutor.Stop();
            ActiveState?.Exit(contextProvider);
        }

    }
}