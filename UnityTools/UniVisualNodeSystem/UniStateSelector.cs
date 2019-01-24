using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Tools.UnityTools.StateMachine.Interfaces;
using UniModule.UnityTools.Interfaces;
using UnityEngine;

namespace UniStateMachine
{
    
    [Serializable]
    [CreateAssetMenu(menuName = "UniStateMachine/States/StateSelector",fileName = "StateSelector")]
    public class UniStateSelector : UniStateTransition
    {

        public List<UniStateTransition> _stateNodes;
        
        public List<UniStateTransition> Nodes => _stateNodes;

        public override bool Validate(IContext context)
        {
            for (int i = 0; i < _stateNodes.Count; i++)
            {
                var state = _stateNodes[i];
                if (state.Validate(context))
                    return true;
            }

            return false;
        }
        
        public override IContextState<IEnumerator> SelectState(IContext context)
        {
            for (var i = 0; i < _stateNodes.Count; i++)
            {
                
                var state = _stateNodes[i];
                if(state == null)
                    continue;
                
                //select transition
                var selectionResult = state.Validate(context);
                
                if (!selectionResult)
                    continue;

                return state.SelectState(context);
                
            }

            return null;
        }

    }
}