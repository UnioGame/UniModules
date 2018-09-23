using System;
using System.Collections;
using Assets.Scripts.Common;
using Assets.Scripts.Tools.StateMachine;
using UniStateMachine;
using UnityEngine;

namespace UniStateMachine {
    
    [Serializable]
    [CreateAssetMenu(menuName = "UniStateMachine/StateNode", fileName = "StateNode")]
    public class UniStateTransition : ScriptableObject ,IValidator<IContextProvider> {

        private bool _defaultValidatorValue = false;
        
        [SerializeField]
        private UniNodeValidator _validator;
        [SerializeField] 
        private UniStateBehaviour _stateBehaviour;

        public UniNodeValidator Validator => _validator;

        public void SetValidator(UniNodeValidator validator) {
            _validator = validator;
        }
        
        public void SetBehaviour(UniStateBehaviour behaviour) {
            _stateBehaviour = behaviour;
        }

        public virtual UniStateBehaviour GetState() {
            return _stateBehaviour;
        }

        public bool Validate(IContextProvider data) {
            
            if (_validator == null)
                return _defaultValidatorValue;
            return _validator.Validate(data);
            
        }
    }
}