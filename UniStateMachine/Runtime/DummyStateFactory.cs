using UniModule.UnityTools.UniStateMachine.Interfaces;

namespace UniModule.UnityTools.UniStateMachine {

    public class DummyStateFactory<TState> : IStateFactory<TState, TState> {

        public TState Create(TState state) {
            return state;
        }

    }
}
