using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Tools.StateMachine;
using Modules.UnityToolsModule.Tools.UnityTools.Interfaces;
using UnityEngine;


namespace UniStateMachine
{

    [CreateAssetMenu(menuName = "UniStateMachine/FSM", fileName = "StateMachine")]
    public class DataContextStateMachine : UniStateMachineObject<ContextStateSelector>
    {
        protected IContextProvider _contextProvider;
        
        public void Initialize(IContextProvider contextProvider)
        {
            _contextProvider = contextProvider;
            _stateSelector.Initialize(contextProvider);
        }

    }

}