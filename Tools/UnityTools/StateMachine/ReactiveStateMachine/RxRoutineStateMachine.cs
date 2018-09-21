using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Tools.StateMachine;
using UnityEngine;

public class RxRoutineStateMachine<TData> : ReactiveStateMachine<IStateBehaviour<IEnumerator>> 
{


    public RxRoutineStateMachine(IStateSelector<IStateBehaviour<IEnumerator>> selector)
    {
        
        var validator = new DummyStateValidator<IStateBehaviour<IEnumerator>>();
        var stateMachine = new StateMachine<IEnumerator>(new RxStateExecutor());
        var stateManager = new StateManager<IStateBehaviour<IEnumerator>,IStateBehaviour<IEnumerator>>(
            stateMachine,new DummyStateFactory<IStateBehaviour<IEnumerator>>(),validator);
        
        Initialize(selector,stateManager);
        
    }
    
}
