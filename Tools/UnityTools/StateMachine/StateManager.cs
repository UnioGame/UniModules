using System;
using System.Collections.Generic;
using System.Diagnostics;
using Assets.Scripts.Extensions;
using Assets.Scripts.ProfilerTools;
using Assets.Scripts.Tools.StateMachine;
using UniRx;
using UniStateMachine;
using Debug = UnityEngine.Debug;

namespace UniStateMachine
{
    public class StateManager<TStateType, TState> : IStateManager<TStateType> {
        
        protected List<IDisposable> _disposables;
        protected IStateValidator<TStateType> _validator;
        protected IStateMachine<TState> _stateMachine;
        private IStateFactory<TStateType, TState> _stateFactory;

        public StateManager(
            IStateMachine<TState> stateMachine,
            IStateFactory<TStateType, TState> stateFactory,
            IStateValidator<TStateType> validator = null)
        {
            _disposables = new List<IDisposable>();
            
            _stateMachine = stateMachine;
            _stateFactory = stateFactory;
            _validator = validator;

        }

        public TStateType CurrentState { get; protected set; }
        public TStateType PreviousState { get; protected set; }


        public virtual void Dispose() {
            
            _disposables.Cancel();
            
        }
        
        public virtual void Stop()
        {
            
            _stateMachine.Stop();
            
        }

        public virtual void SetState(TStateType state)
        {
            ExecuteState(state);
        }

        protected void ExecuteState(TStateType state)
        {
            if (!ValidateTransition(state))
                return;
            PreviousState = CurrentState;

            StateLogger.LogStateChanged(this?.GetType().Name,
                CurrentState?.GetType().Name,state?.GetType().Name);
            
            CurrentState = state;
            ChangeState(state);
        }

        private void ChangeState(TStateType state)
        {
            if (state == null) {
                _stateMachine.Stop();
                return;
            }
            var stateBehaviour = _stateFactory.Create(state);
            _stateMachine.Execute(stateBehaviour);
        }

        protected virtual bool ValidateTransition(TStateType nextState)
        {
            return _validator == null || 
                   _validator.Validate(CurrentState, nextState);
        }

    }
}