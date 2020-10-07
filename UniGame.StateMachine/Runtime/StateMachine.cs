namespace UniModules.UniStateMachine.Runtime
{
    using Interfaces;

    public class StateMachine<TAwaiter> :
        BaseStateMachine<IStateBehaviour<TAwaiter>>
    {

        #region constructor
        
        public StateMachine(IStateExecutor<IStateBehaviour<TAwaiter>> executor) : 
            base(executor) {}
        
        #endregion

        #region private methods

        protected override void OnExit() {
            State = null;
        }

        #endregion

    }
}
