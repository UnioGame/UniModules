﻿using System;
using System.Diagnostics;
using Assets.Scripts.Extensions;
using Assets.Scripts.ProfilerTools;
using UniRx;

namespace Assets.Scripts.Tools.StateMachine.AsyncStateMachine
{
    public class AsyncStateManager<TStateType> : IStateManager<TStateType>
    {
        protected IStateTransitionValidator<TStateType> _validator;
        private readonly IStateController<TStateType> _stateController;
        protected IAsyncStateMachine _stateMachine;
        private readonly IAsyncStateFactory<TStateType> _stateFactory;
        protected readonly IDisposable _controllerDisposable;

        #region constructor

        public AsyncStateManager(IStateController<TStateType> stateController,
            IAsyncStateMachine stateMachine,
            IAsyncStateFactory<TStateType> stateFactory,
            IStateTransitionValidator<TStateType> validator = null)
        {
            _stateController = stateController;
            _stateMachine = stateMachine;
            _stateFactory = stateFactory;
            _validator = validator;
            _controllerDisposable = 
                _stateController.StateObservable.
                Skip(1).Subscribe(ExecuteState);
        }

        #endregion

        #region public properties


        public TStateType CurrentState { get; protected set; }


        public TStateType PreviousState { get; protected set; }


        #endregion

        #region public methods

        public virtual void Dispose()
        {
            _stateMachine.Dispose();
            _controllerDisposable.Cancel();
        }

        public void SetState(TStateType state)
        {
            _stateController.SetState(state);
        }

        #endregion

        #region private methods
        
        protected void ExecuteState(TStateType state)
        {
            if (!ValidateTransition(state))
                return;
            PreviousState = CurrentState;
            CurrentState = state;
            ChangeState(state);
        }

        protected virtual void ChangeState(TStateType state)
        {
            var stateBehaviour = _stateFactory.Create(state);
            _stateMachine.Execute(stateBehaviour);
        }

        protected virtual bool ValidateTransition(TStateType nextState)
        {
            return _validator == null || 
                _validator.Validate(CurrentState, nextState);
        }

        #endregion

    }
}
