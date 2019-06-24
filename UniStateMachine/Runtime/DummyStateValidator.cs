namespace UniGreenModules.UniStateMachine.Runtime {
    using Interfaces;


    public class DummyStateValidator<TState> : IStateValidator<TState>
        where TState : class {

        public bool Validate(TState fromState, TState toState) {
            if (fromState == toState)
                return false;
            return true;

        }

    }
}
