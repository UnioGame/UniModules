using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Tools.StateMachine;
using UnityEngine;

namespace Assets.Scripts.Tools.StateMachine {
    
    public class ActorRoutineStateMachine<TData> : BaseStateMachine<IStateBehaviour<TData, IEnumerator>> {

        public ActorRoutineStateMachine(
            IStateExecutor<IStateBehaviour<TData, IEnumerator>> executor) :
            base(executor) {
        }

    }
}
