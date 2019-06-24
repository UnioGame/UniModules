namespace UniGreenModules.UniStateMachine.Runtime
{
    using System;
    using Interfaces;
    using UniRx;

    public class ControllerStateManager<TStateType,TState> : StateManager<TStateType, TState>
    {
        private readonly IStateController<TStateType> _stateController;
        private IDisposable _controllerDisposable;
        
        #region constructor

        public ControllerStateManager(IStateController<TStateType> stateController,
            IStateMachine<TState> stateMachine,
            IStateFactory<TStateType, TState> stateFactory,
            IStateValidator<TStateType> validator = null) : 
            base(stateMachine, stateFactory, validator)
        {
            _stateController = stateController;
            
            ObservableExtensions.Subscribe<TStateType>(_stateController.StateObservable.
                    Skip(1), ExecuteState).AddTo(_disposables);
            
        }

        #endregion

        public override void SetState(TStateType state)
        {
            _stateController.SetState(state);
        }
    }
}
