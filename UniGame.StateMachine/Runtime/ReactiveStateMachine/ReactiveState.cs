﻿namespace UniModules.UniStateMachine.Runtime.ReactiveStateMachine
{
    using System.Collections;
    using Interfaces;
    using global::UniGame.Core.Runtime;
    using UniRx;

    public class ReactiveState<TState> : StateBehaviour
    {
        private ISelector<TState> _stateSelector;
        private IStateManager<TState> _stateManager;
        
        public void Initialize(ISelector<TState> stateSelector,
            IStateManager<TState> stateManager)
        {
            if(IsActive.Value)
                Exit();
            
            _stateSelector = stateSelector;
            _stateManager = stateManager;          
            _stateManager.AddTo(_disposables);

        }
        

        #region private methods

        protected override IEnumerator ExecuteState()
        {
            while (IsActive.Value)
            {
                var state = _stateSelector.Select();
                
                _stateManager.SetState(state);
                
                yield return null;
            }
        }

        protected override void OnExite()
        {
            _stateManager = null;
            _stateSelector = null;
            base.OnExite();
        }

        #endregion
    }
}