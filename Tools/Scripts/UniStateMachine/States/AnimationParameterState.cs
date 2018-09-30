using System.Collections;
using System.Collections.Generic;
using UniStateMachine;
using UnityEngine;

namespace GamePlay.States {

    public struct AnimatorParameterValue {

        public AnimatorControllerParameterType ParameterType;
        public string Name;
        public float FloatValue;
        public int IntValue;
        public bool BoolValue;
        
    }
    
    public class AnimationParameterState : UniStateBehaviour {

        [SerializeField]
        private List<AnimatorParameterValue> _parameters = new List<AnimatorParameterValue>();
        
        private int _animationId;

        protected override IEnumerator ExecuteState() {

            var animator = _contextProvider.GetContext<Animator>();

            var parameters = animator.parameters;

            for (int i = 0; i < _parameters.Count; i++) {
                
                var parameter = _parameters[0];
                
            }
            
            yield break;

        }

    }

}
