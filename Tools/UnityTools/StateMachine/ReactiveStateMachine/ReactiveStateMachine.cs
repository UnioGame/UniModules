using System.Collections;
using Assets.Scripts.Tools.StateMachine;
using UniRx;

namespace UniStateMachine
{
    public class ReactiveStateMachine<TState> : StateBehaviour
    {
        private IStateSelector<TState> _stateSelector;
        private IStateManager<TState> _stateManager;
        
        public void Initialize(IStateSelector<TState> stateSelector,
            IStateManager<TState> stateManager)
        {
            if(IsActive)
                Exit();
            
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