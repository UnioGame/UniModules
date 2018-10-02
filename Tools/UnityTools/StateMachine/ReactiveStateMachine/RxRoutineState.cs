using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Tools.StateMachine;
using UnityEngine;

namespace UniStateMachine {

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