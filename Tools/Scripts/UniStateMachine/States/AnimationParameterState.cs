using System;
using System.Collections;
using System.Collections.Generic;
using UniStateMachine;
using UnityEngine;
using UnityToolsModule.Tools.UnityTools.Animator;

namespace GamePlay.States {


    public class AnimationParameterState : UniStateBehaviour {

        [SerializeField]
        private List<AnimatorParameter> _parameters = new List<AnimatorParameter>();
        
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
