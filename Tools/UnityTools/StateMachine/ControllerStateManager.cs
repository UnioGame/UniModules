using System;
using System.Diagnostics;
using Assets.Scripts.Extensions;
using UniRx;
using Debug = UnityEngine.Debug;

namespace Assets.Scripts.Tools.StateMachine
{
    public class ControllerStateManager<TStateType,TAwaiter> : StateManager<TStateType, TAwaiter>
    {
        private readonly IStateController<TStateType> _stateController;

        #region constructor

        public ControllerStateManager(IStateController<TStateType> stateController,
            IStateMachine<IStateBehaviour<TAwaiter>> stateMachine,
            IStateFactory<TStateType, TAwaiter> stateFactory,
            IStateValidator<TStateType> validator = null) : 
            base(stateMachine, stateFactory, validator)
        {
            _stateController = stateController;
            ObservableExtensions.Subscribe<TStateType>(_stateController.StateObservable.
                    Skip(1), ExecuteState).AddTo(_disposables);
        }

        #endregion

        public override void Dispose()
        {
            base.Dispose();
            _stateController.Cancel();
        }

        public override void SetState(TStateType state)
        {
            _stateController.SetState(state);
        }
    }
}
