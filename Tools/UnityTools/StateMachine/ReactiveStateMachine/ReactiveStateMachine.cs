using System.Collections;
using UniRx;

namespace Assets.Scripts.Tools.StateMachine
{
    public class ReactiveStateMachine<TAwaiter> : StateBehaviour
    {
        private IStateSelector<IStateBehaviour<TAwaiter>> _stateSelector;
        private IStateManager<IStateBehaviour<TAwaiter>> _stateManager;
        
        public void Initialize(IStateSelector<IStateBehaviour<TAwaiter>> stateSelector,
            IStateManager<IStateBehaviour<TAwaiter>> stateManager)
        {
            if(IsActive)
                Stop();
            
            _stateSelector = stateSelector;
            _stateManager = stateManager;          
            _stateManager.AddTo(_disposables);

        }
        

        #region private methods

        protected override IEnumerator ExecuteState()
        {
            while (IsActive)
            {
                var state = _stateSelector.Select();
                
                _stateManager.SetState(state);
                
                yield return null;
            }
        }

        protected override void OnStateStop()
        {
            _stateManager = null;
            _stateSelector = null;
            base.OnStateStop();
        }

        #endregion
    }
}