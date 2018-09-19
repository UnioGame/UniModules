using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Tools.StateMachine;
using UnityEngine;

namespace Assets.Scripts.Tools.StateMachine {
    
    public interface IStateBehaviour<TData, TResult> : 
        
        IRoutine<TData, TResult> {

        bool IsActive { get; }
        
        void Exit();

    }
    
}
