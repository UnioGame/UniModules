using System;
using Assets.Tools.UnityTools.Interfaces;
using UnityEngine;

namespace Assets.Tools.UnityTools.StateMachine.UniStateMachine {
    
    [Serializable]
    [CreateAssetMenu(menuName = "UniStateMachine/StateNode", fileName = "StateNode")]
    public class UniStateTransition : ScriptableObject ,IValidator<IContext> {

        private bool _defaultValidatorValue = false;
        
        [SerializeField]
        private UniTransitionValidator _validator;
        
        [SerializeField] 
        private UniStateBehaviour _stateBehaviour;

        public UniTransitionValidator Validator => _validator;

        public void SetValidator(UniTransitionValidator validator) {
            _validator = validator;
        }
        
        public void SetBehaviour(UniStateBehaviour behaviour) {
            _stateBehaviour = behaviour;
        }

        public virtual UniStateBehaviour GetState() {
            return _stateBehaviour;
        }

        public bool Validate(IContext data) {
            
            if (_validator == null)
                return _defaultValidatorValue;
            return _validator.Validate(data);
            
        }
    }
}