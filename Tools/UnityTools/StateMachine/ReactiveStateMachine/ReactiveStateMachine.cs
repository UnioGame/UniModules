using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;

namespace Assets.Scripts.Tools.StateMachine
{
    public class ReactiveStateMachine<TAwaiter> : StateBehaviour
    {
        private readonly IStateSelector<IStateBehaviour<TAwaiter>> _stateSelector;
        private readonly IStateValidator<IStateBehaviour<TAwaiter>> _stateValidator;
        private readonly IStateFactory<IStateBehaviour<TAwaiter>, TAwaiter> _stateFactory;
        private readonly IStateMachine<IStateBehaviour<TAwaiter>> _stateMachine;
        private readonly IStateManager<IStateBehaviour<TAwaiter>> _stateManager;
        
        protected readonly List<IDisposable> _disposables;
            
        #region constructors

        public ReactiveStateMachine(
            IStateSelector<IStateBehaviour<TAwaiter>> stateSelector,
            IStateMachine<IStateBehaviour<TAwaiter>> stateMachine,
            IStateFactory<IStateBehaviour<TAwaiter>,TAwaiter> stateFactory,
            IStateValidator<IStateBehaviour<TAwaiter>> stateValidator)
        {
            
            _disposables = new List<IDisposable>();

            _stateSelector = stateSelector;
            _stateMachine = stateMachine;
            _stateFactory = stateFactory;
            _stateValidator = stateValidator;

            _stateManager = new StateManager<IStateBehaviour<TAwaiter>,TAwaiter>(
                stateMachine,stateFactory,stateValidator);

            _stateMachine.AddTo(_disposables);

        }
        

        #endregion

        
        public bool IsActive { get; protected set; }

        #region public methods


        #endregion
        
        #region private methods

        protected override IEnumerator ExecuteState()
        {
            while (_isActive)
            {
                var state = _stateSelector.Select();
                
                _stateManager.SetState(state);
                
                yield return null;
            }
        }

        #endregion
    }
}