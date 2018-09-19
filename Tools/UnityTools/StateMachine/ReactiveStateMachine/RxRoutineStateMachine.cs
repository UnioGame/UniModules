using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Tools.StateMachine;
using UnityEngine;

public class RxRoutineStateMachine : ReactiveStateMachine<IEnumerator> {


    public RxRoutineStateMachine(IStateSelector<IStateBehaviour<IEnumerator>> selector)
    {
        
        var validator = new DummyStateValidator<IStateBehaviour<IEnumerator>>();
        var stateMachine = new StateMachine<IEnumerator>(new RxStateExecutor());
        var stateManager = new StateManager<IStateBehaviour<IEnumerator>,IEnumerator>(
            stateMachine,new DummyStateFactory<IEnumerator>(),validator);
        
        Initialize(selector,stateManager);
        
    }
    
}
