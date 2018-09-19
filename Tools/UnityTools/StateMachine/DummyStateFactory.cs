using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Tools.StateMachine;
using UnityEngine;

public class DummyStateFactory<TState> : IStateFactory<TState,TState> {
    
    public TState Create(TState state)
    {
        return state;
    }
    
}
