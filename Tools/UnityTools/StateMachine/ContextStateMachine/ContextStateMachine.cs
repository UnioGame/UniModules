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
            
        private readonly IContextExecutor<TAwaiter> _stateExecutor;

        private IRoutineExecutor<TAwaiter> _stateBehaviour;
        private IDisposable _stateExecution;

        protected List<IDisposable> disposables;


        #region public methods

        public ContextStateMachine(IContextExecutor<TAwaiter> stateExecutor)
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
            StopActiveState();
            ChangeState(state, context);
        }

        public virtual void Stop()
        {
            StopActiveState();
        }

        #endregion

        #region public properties

        public IContextStateBehaviour<TAwaiter> ActiveState { get; protected set; }

        public IContextProvider Context { get; protected set; }

        #endregion

        private void ChangeState(IContextStateBehaviour<TAwaiter> state, 
            IContextProvider context)
        {

            if (state == null)
            {
                ActiveState = null;
                Context = null;
                return;
            }

            ActiveState = state;
            Context = context;

            StartState(state, context);
            
        }

        private void StartState(IContextStateBehaviour<TAwaiter> state, IContextProvider context)
        {

            var routine = state.Execute(context);
            _stateExecutor.Execute(routine);
            
        }
      
        private void StopActiveState()
        {
            _stateExecution.Cancel();
            ActiveState?.Exit(Context);
        }

    }
}