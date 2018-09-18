using System;
using System.Diagnostics;
using Assets.Scripts.Extensions;
using UniRx;
using Debug = UnityEngine.Debug;

namespace Assets.Scripts.Tools.StateMachine
{
    public class StateManager<TStateType,TAwaiter> : IStateManager<TStateType>
    {
        protected IStateTransitionValidator<TStateType> _validator;
        private readonly IStateController<TStateType> _stateController;
        protected IStateMachine<IStateBehaviour<TAwaiter>> _stateMachine;
        private readonly IStateFactory<TStateType, TAwaiter> _stateFactory;
        protected readonly IDisposable _controllerDisposable;

        #region constructor

        public StateManager(IStateController<TStateType> stateController,
            IStateMachine<IStateBehaviour<TAwaiter>> stateMachine,
            IStateFactory<TStateType, TAwaiter> stateFactory,
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
