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
        [SerializeField]
        private UniStateTransition _selectedTransition;
        [SerializeField]
        private UniStateBehaviour _selectStateBehaviour;

        [Reorderable]
        public List<UniStateTransition> _stateNodes;
        
        public List<UniStateTransition> Nodes => _stateNodes;

        public UniStateTransition SelectTransition => _selectedTransition;

        public UniStateBehaviour SelectState => _selectStateBehaviour;

        public virtual IContextStateBehaviour<IEnumerator> Select(IContext context)
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

                _selectedTransition = state;
                _selectStateBehaviour = behaviour;

                return behaviour;
                
            }

            return null;
        }

    }
}