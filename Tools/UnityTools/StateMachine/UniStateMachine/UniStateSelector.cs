using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Tools.StateMachine;
using Modules.UnityToolsModule.Tools.UnityTools.Interfaces;
using SubjectNerd.Utilities;
using UnityEngine;

namespace UniStateMachine
{
    
    [Serializable]
    public class UniStateSelector :ScriptableObject,
        IStateSelector<IStateBehaviour<IEnumerator>>
    {
        private IContextProvider _contextProvider;
        
        [Reorderable]
        public List<UniStateTransition> _stateNodes;
        
        public List<UniStateTransition> Nodes => _stateNodes;
        
        
        public void Initialize(IContextProvider contextProvider)
        {
            _contextProvider = contextProvider;
        }
        
        
        public virtual IStateBehaviour<IEnumerator> Select()
        {
            for (int i = 0; i < _stateNodes.Count; i++)
            {
                
                var state = _stateNodes[i];
                if(state == null)
                    continue;
                if (!state.Validate(_contextProvider))
                    continue;
                
                var behaviour = state.GetState();
                behaviour.Initialize(_contextProvider);
                return behaviour;
                
            }

            return null;
        }

    }
}