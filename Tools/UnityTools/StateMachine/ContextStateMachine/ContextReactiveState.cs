using System.Collections;
using Modules.UnityToolsModule.Tools.UnityTools.Interfaces;
using Assets.Modules.UnityToolsModule;

namespace StateMachine.ContextStateMachine
{
    public class ContextReactiveState : ContextStateBehaviour
    {
        private IContextSelector<IEnumerator> _stateSelector;
        private IContextStateMachine<IEnumerator> _stateMachine;

        public void Initialize(IContextSelector<IEnumerator> stateSelector,
            IContextStateMachine<IEnumerator> stateManager)
        {
            Dispose();

            _stateSelector = stateSelector;
            _stateMachine = stateManager;

        }


        #region private methods

        protected override IEnumerator ExecuteState(IContextProvider context)
        {
            while (IsActive(context))
            {
                var state = _stateSelector.Select(context);

                if (ValidateState(state, context))
                {
                    _stateMachine.Execute(state, context);
                }

                yield return null;
            }
        }

        protected override void OnExit(IContextProvider context)
        {
            _stateMachine = null;
            _stateSelector = null;
        }

        /// <summary>
        /// is state with target context already active, when return false
        /// </summary>
        private bool ValidateState(IContextStateBehaviour<IEnumerator> state, IContextProvider context)
        {
            var activeState = _stateMachine.ActiveState;

            if (activeState == null || _stateMachine.IsActive == false)
                return true;

            if (activeState == state && activeState.IsActive(context))
                return false;

            return true;
        }

        #endregion
    }
}
