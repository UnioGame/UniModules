using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Tools.UnityTools.Interfaces;
using Assets.Tools.UnityTools.ReorderableInspector;
using Assets.Tools.UnityTools.StateMachine.Interfaces;
using UnityEngine;

namespace Assets.Tools.UnityTools.StateMachine.UniStateMachine
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