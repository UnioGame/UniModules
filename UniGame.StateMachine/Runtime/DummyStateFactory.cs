namespace UniModules.UniStateMachine.Runtime {
    using Interfaces;

    public class DummyStateFactory<TState> : IStateFactory<TState, TState> {

        public TState Create(TState state) {
            return state;
        }

    }
}
