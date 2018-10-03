using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Tools.UnityTools.Animator;
using Assets.Tools.UnityTools.Interfaces;
using Assets.Tools.UnityTools.StateMachine.UniStateMachine;
using UnityEngine;

namespace GamePlay.States {


    public class AnimationParameterState : UniStateBehaviour {

        [SerializeField]
        private List<AnimatorParameter> _parameters = new List<AnimatorParameter>();
        
        private int _animationId;

        protected override IEnumerator ExecuteState(IContext context) {

            var animator = context.Get<Animator>();

            var parameters = animator.parameters;

            for (int i = 0; i < _parameters.Count; i++) {
                
                var parameter = _parameters[0];
                
            }
            
            yield break;

        }

    }

}
