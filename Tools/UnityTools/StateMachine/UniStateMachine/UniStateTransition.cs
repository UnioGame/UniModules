using System;
using System.Collections;
using Assets.Tools.UnityTools.Interfaces;
using UnityEngine;

namespace Assets.Tools.UnityTools.StateMachine.UniStateMachine {
    
    [Serializable]
    [CreateAssetMenu(menuName = "States/Transitions/StateTransition", fileName = "StateTransition")]
    public class UniStateTransition : UniStateBehaviour, IValidator<IContext> {

        private bool _defaultValidatorValue = false;
        
        [SerializeField]
        private UniTransitionValidator _validator;
        
        [SerializeField] 
        private UniStateBehaviour _stateBehaviour;

        public UniTransitionValidator Validator => _validator;

        #region public methods

        public void SetValidator(UniTransitionValidator validator) {
            _validator = validator;
        }
        
        public void SetBehaviour(UniStateBehaviour behaviour) {
            _stateBehaviour = behaviour;
        }

        public virtual UniStateBehaviour GetState() {
            return _stateBehaviour;
        }

        public bool Validate(IContext data)
        {

            if (_validator == null)
                return _defaultValidatorValue;
            return _validator.Validate(data);

        }

        #endregion

        protected override IEnumerator ExecuteState(IContext context)
        {
            if(Validate(context) == false)
                yield break;
            var behaviour = GetState();
            if(!behaviour)
                yield break;

            yield return behaviour.Execute(context);
        }

        protected override void OnExit(IContext context) 
        {
            _stateBehaviour?.Exit(context);
            base.OnExit(context);
        }
    }
}