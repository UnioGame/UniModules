using System.Collections;
using Assets.Tools.UnityTools.Interfaces;
using Assets.Tools.UnityTools.StateMachine.Interfaces;

namespace Assets.Tools.UnityTools.StateMachine.ContextStateMachine
{

    /// <summary>
    /// reactive state must be one item per execution instance
    /// </summary>
    public class ContextReactiveStateMachine : ContextStateBehaviour
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

        public override bool IsActive(IContext context) {

            var state = _stateMachine?.ActiveState;
            return state != null && state.IsActive(context);

        }

        public override void Dispose()
        {
            _stateMachine?.Stop();
            _stateMachine = null;
            _stateSelector = null;
            base.Dispose();
        }

        #region private methods

        protected override IEnumerator ExecuteState(IContext context)
        {
            while (_stateSelector!=null && _stateMachine !=null)
            {
                var state = _stateSelector.Select(context);

                if (ValidateState(state, context))
                {
                    _stateMachine.Execute(state, context);
                }

                yield return null;
            }
        }

        /// <summary>
        /// stop state machine at any exit call
        /// </summary>
        protected override void OnExit(IContext context)
        {
            Dispose();
        }

        /// <summary>
        /// is state with target context already active, when return false
        /// </summary>
        private bool ValidateState(IContextState<IEnumerator> state, IContext context)
        {
            var activeState = _stateMachine.ActiveState;
            var stateContext = _stateMachine.Context;

            if (activeState == null)
                return true;
            if (activeState == state && stateContext == context)
                return false;

            return true;
        }

        #endregion
    }
}
