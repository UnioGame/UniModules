using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Scripts.Tools.StateMachine
{

    [CreateAssetMenu(menuName = "UniStateMachine/DataContext StateMachine", fileName = "DataContextStateMachine")]
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