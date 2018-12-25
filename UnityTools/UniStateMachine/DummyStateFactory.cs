using Assets.Tools.UnityTools.StateMachine.Interfaces;

namespace Assets.Tools.UnityTools.StateMachine {

    public class DummyStateFactory<TState> : IStateFactory<TState, TState> {

        public TState Create(TState state) {
            return state;
        }

    }
}
