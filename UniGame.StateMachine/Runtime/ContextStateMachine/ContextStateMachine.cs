namespace UniModules.UniStateMachine.Runtime.ContextStateMachine
{
    using System;
    using System.Collections.Generic;
    using Interfaces;
    using UniCore.Runtime.Extension;
    using UniGame.Core.Runtime.Interfaces;

    /// <summary>
    /// FSM for supports states with context data
    /// one per actor execution process
    /// </summary>
    /// <typeparam name="TAwaiter">awaiter type</typeparam>
    public class ContextStateMachine<TAwaiter> : 
        IContextStateMachine<TAwaiter>
    {
            
        private readonly IContextExecutor<TAwaiter> _stateExecutor;
        private IRoutineExecutor<TAwaiter> _stateBehaviour;
        private IDisposableItem _stateExecution;

        protected List<IDisposable> disposables;


        public ContextStateMachine(IContextExecutor<TAwaiter> stateExecutor)
        {
            disposables = new List<IDisposable>();
            _stateExecutor = stateExecutor;
        }
        
        #region public methods

        /// <summary>
        /// stop active execution
        /// call relative dispose methods
        /// </summary>
        public virtual void Dispose() {

            StopActiveState();
            disposables.Cancel();
            
        }

        public void Execute(IContextState<TAwaiter> state, IContext context)
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

        public IContextState<TAwaiter> ActiveState { get; protected set; }

        public IContext Context { get; protected set; }

        public bool IsActive => _stateExecution != null && _stateExecution.IsComplete == false;

        #endregion

        private void ChangeState(IContextState<TAwaiter> state, 
            IContext context)
        {

            if (state == null)
            {
                return;
            }

            ActiveState = state;
            Context = context;

            StartState(state, context);
            
        }

        private void StartState(IContextState<TAwaiter> state, IContext context)
        {

            var routine = state.Execute(context);
            _stateExecution = _stateExecutor.Execute(routine);
            
        }
      
        private void StopActiveState()
        {
            _stateExecution?.Dispose();
            ActiveState?.Exit();
            
            ActiveState = null;
            Context = null;
            _stateExecution = null;
        }

    }
}