using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Tools.StateMachine;
using UnityEngine;

public class DummyStateFactory<TAwaiter> : IStateFactory<IStateBehaviour<TAwaiter>,TAwaiter> {
    
    public IStateBehaviour<TAwaiter> Create(IStateBehaviour<TAwaiter> state)
    {
        return state;
    }
    
}
