namespace UniModules.UniStateMachine.Runtime.ReactiveStateMachine {
    using System.Collections;
    using Interfaces;
    using UniCore.Runtime.Interfaces;

    public class RxRoutineState<TData> : ReactiveState<IStateBehaviour<IEnumerator>> {


        public RxRoutineState(ISelector<IStateBehaviour<IEnumerator>> selector) {

            var validator = new DummyStateValidator<IStateBehaviour<IEnumerator>>();
            var stateMachine = new StateMachine<IEnumerator>(new RxStateExecutor());
            var stateManager = new StateManager<IStateBehaviour<IEnumerator>, IStateBehaviour<IEnumerator>>(
                stateMachine, new DummyStateFactory<IStateBehaviour<IEnumerator>>(), validator);

            Initialize(selector, stateManager);

        }

    }
}