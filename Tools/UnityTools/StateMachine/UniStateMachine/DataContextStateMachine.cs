using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Scripts.Tools.StateMachine
{

    [CreateAssetMenu(menuName = "UniStateMachine/FSM", fileName = "StateMachine")]
    public class DataContextStateMachine : UniStateMachineObject<DataContextStateSelector>
    {
        protected IContextProvider _contextProvider;
        
        public void Initialize(IContextProvider contextProvider)
        {
            _contextProvider = contextProvider;
            _stateSelector.Initialize(contextProvider);
        }

    }

}