using System.Collections;
using UniModule.UnityTools.Interfaces;
using UniModule.UnityTools.UniStateMachine.Interfaces;

namespace UniModule.UnityTools.UniStateMachine.ReactiveStateMachine {

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