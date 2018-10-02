using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Modules.UnityToolsModule;
using Modules.UnityToolsModule.Tools.UnityTools.Interfaces;
using StateMachine.ContextStateMachine;
using SubjectNerd.Utilities;
using UnityEngine;

namespace UniStateMachine
{
    
    [Serializable]
    [CreateAssetMenu(menuName = "UniStateMachine/StateSelector",fileName = "StateSelector")]
    public class UniStateSelector :ScriptableObject,
        IContextSelector<IEnumerator>
    {
        [Reorderable]
        public List<UniStateTransition> _stateNodes;
        
        public List<UniStateTransition> Nodes => _stateNodes;
        
        public virtual IContextStateBehaviour<IEnumerator> Select(IContextProvider context)
        {
            for (int i = 0; i < _stateNodes.Count; i++)
            {
                
                var state = _stateNodes[i];
                if(state == null)
                    continue;
                
                //select transition
                var selectionResult = state.Validate(context);
                
                if (!selectionResult)
                    continue;
                
                //get transition state
                var behaviour = state.GetState();
                return behaviour;
                
            }

            return null;
        }

    }
}